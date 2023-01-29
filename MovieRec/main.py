# dataset: https://grouplens.org/datasets/movielens/
import os
from ast import literal_eval
import pandas as pd 
import re
from gensim import models
from gensim import similarities

def weight_rating(data, min_votes, vote_average):
    m_votes_number = data['vote_count']
    m_avarage_rating = data['vote_average']
    return ( m_votes_number / ( m_votes_number + min_votes ) * m_avarage_rating ) + ( min_votes / ( min_votes + m_votes_number ) * vote_average )
    
def top_movies(genre, min_persentage = 0.60):
    current_dataset = gen_movie_dataset[gen_movie_dataset['genre'] == genre]
    
    vote_count = current_dataset[current_dataset['vote_count'].notnull()]['vote_count'].astype('int')
    vote_avarages = current_dataset[current_dataset['vote_average'].notnull()]['vote_average'].astype('int')
    
    vote_average = vote_avarages.mean()
    min_votes = vote_count.quantile(min_persentage)

    qualified = current_dataset[(current_dataset['vote_count'] >= min_votes) & (current_dataset['vote_count'].notnull())][['title', 'year', 'vote_count', 'vote_average', 'popularity', 'imdb_id']]
    
    qualified['vote_count'] = qualified['vote_count'].astype('int')
    qualified['vote_average'] = qualified['vote_average'].astype('int')

    qualified['weight_rating'] = qualified.apply(lambda movie_data: weight_rating(movie_data, min_votes, vote_average), axis = 1)
    qualified = qualified.sort_values('weight_rating', ascending = False).head(10)
    return qualified

for dirname, _, filenames in os.walk('./movies-dataset'):
    for filename in filenames:
        print(os.path.join(dirname, filename))

pd.pandas.set_option('display.max_columns', None) 
movie_dataset = pd.read_csv('./movies-dataset/movies_metadata.csv')
#print(movie_dataset.head())

# fillna => replace Nan with []
# literal_eval => read as a tuple the gendre (int and string)
movie_dataset['genres'] = movie_dataset['genres'].fillna('[]').apply(literal_eval)
#movie_dataset['genres'].apply(lambda idNamePair: print(idNamePair))   
movie_dataset['genres'] = movie_dataset['genres'].apply(lambda idNamePair: [entry['name'] for entry in idNamePair])   #keep only the genre names
#print(movie_dataset['genres'])

vote_count = movie_dataset[movie_dataset['vote_count'].notnull()]['vote_count'].astype('int')
#print(vote_count)

vote_averages = movie_dataset[movie_dataset['vote_average'].notnull()]['vote_average'].astype('int')
#print(vote_averages)

vote_average = vote_averages.mean()
#print(vote_average)
#print(vote_count.sort_values(ascending = False))
min_votes = vote_count.quantile(0.80)  # Minimum votes required, over 80%
#print(min_votes)

pd.to_datetime(movie_dataset['release_date'], errors='coerce')
#print(movie_dataset['release_date'])

movie_dataset['year'] = pd.to_datetime(movie_dataset['release_date'], errors='coerce').apply(lambda date: str(date).split('-')[0]) # y-m-d format
#print(movie_dataset['year'])

qualified = movie_dataset[(movie_dataset['vote_count'] >= min_votes) & (movie_dataset['vote_count'].notnull())][['title', 'year', 'vote_count', 'vote_average', 'popularity', 'genres', 'imdb_id']]
qualified['vote_count'] = qualified['vote_count'].astype('int')
qualified['vote_average'] = qualified['vote_average'].astype('int')
#print(qualified)

# axis {0 or "index", 1 or "columns"}, defaul 0
qualified['weight_rating'] = qualified.apply(lambda movie_data: weight_rating(movie_data, min_votes, vote_average), axis = 1)
#print(qualified.head())

qualified = qualified.sort_values('weight_rating', ascending = False) #.head(250)
#print(qualified.head(15))

# Take the genres column and make it as a series the stack(). Function will make the list of geners for movie as a stack and having the same index for the geners of the specific movie
#movie_dataset_per_genre = movie_dataset.apply(lambda movie_data: print(movie_data['genres']), axis = 1)
movie_dataset_per_genre = movie_dataset.apply(lambda movie_data: pd.Series(movie_data['genres']), axis = 1).stack().reset_index(level = 1, drop = True)
movie_dataset_per_genre.name = 'genre'
#print(movie_dataset_per_genre)

gen_movie_dataset = movie_dataset.drop('genres', axis = 1).join(movie_dataset_per_genre)
#print(movie_dataset)
#print(gen_movie_dataset)

#-----------------------------------------------------

# Movie Overviews and Taglines
# Movie Discription Based Recommender
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import linear_kernel, cosine_similarity

# Remove 3 entities with wrong data
movie_dataset = movie_dataset.drop([19730, 29503, 35587])

link_small = pd.read_csv('./movies-dataset/links_small.csv')
#print(link_small.head())

# Remove null appearances
link_small = link_small[link_small['tmdbId'].notnull()]['tmdbId'].astype('int')
movie_dataset['id'] = movie_dataset['id'].astype('int')
#print(link_small.head())

# Link them together
smd = movie_dataset[movie_dataset['id'].isin(link_small)]
#print(smd.shape)

# Combine movies overview and tagline as dataset
#print(smd['tagline'])
#print(smd['overview'])
smd['tagline'] = smd['tagline'].fillna('')
smd['description'] = smd['overview'] + smd['tagline']
smd['description'] = smd['description'].fillna('')

description_dataset = smd

# analyzer can be word, number, characters
# ngram_range indicates group the letters in unigram and bigram (pairs of adjacent words).
tf = TfidfVectorizer(analyzer='word', ngram_range = (1,2), min_df = 0, stop_words = 'english')
tfid_mat = tf.fit_transform(smd['description'])

# Prepare a list of similarities
cos_sim = linear_kernel(tfid_mat, tfid_mat)
#print(len(cos_sim[0]))

titles = smd['title'] + smd['imdb_id']

smd = smd.reset_index()
indices = pd.Series(smd.index, index = smd['title'])    # A map between titles and movies index
#print(indices)

def get_recommendations_overview(title):
    idx = indices[title]                                                # Get the index of the movie
    sim_scores = list(enumerate(cos_sim[idx]))                          # Find the cos similarity of the movie using it index and enumarating the similarity
    #print(sim_scores)

    sim_scores = sorted(sim_scores, key=lambda x: x[1], reverse=True)   # Sort the movie based on the similarity score 
    sim_scores = sim_scores[1:31]                                       # Take first 30 movies (first one is itself)
    #print(sim_scores)

    movie_indices = [i[0] for i in sim_scores]                          # Take only the indices for the sorted pairs
    return titles.iloc[movie_indices]


#-------------------
# Movie Cast, Crew, Keywords and Genre

import numpy as np
from nltk.stem.snowball import SnowballStemmer
from sklearn.feature_extraction.text import TfidfVectorizer, CountVectorizer

def director_name(crew):
    for job in crew:
        if job['job'] == 'Director':
            return job['name']
    return np.nan

# Get the most important keywords and remove rest
def keep_most_important_keywords(keywords_list, accepted_keywords):
    keywords = []
    for keyword in keywords_list:
        if keyword in accepted_keywords:
            keywords.append(keyword)
    return keywords

credits = pd.read_csv('./movies-dataset/credits.csv')
keyword = pd.read_csv('./movies-dataset/keywords.csv')

# Change the data type as Int
keyword['id'] = keyword['id'].astype('int')
credits['id'] = credits['id'].astype('int')
movie_dataset['id'] = movie_dataset['id'].astype('int')

# Merge the keyword and credits dataset to md dataset based on its ID
movie_dataset = movie_dataset.merge(keyword, on = 'id')
movie_dataset = movie_dataset.merge(credits, on = 'id')

# Link them together
smd = movie_dataset[movie_dataset['id'].isin(link_small)]
#print(smd)

# Transform them into tuples 
smd['cast'] = smd['cast'].apply(literal_eval)
smd['crew'] = smd['crew'].apply(literal_eval)
smd['keywords'] = smd['keywords'].apply(literal_eval)

smd['cast_size'] = smd['cast'].apply(lambda x: len(x))
smd['crew_size'] = smd['crew'].apply(lambda x: len(x))

#print(smd['cast'][0:1][0])         # First row cast details
#print(len(smd['cast'][0:1][0]))    # Length  of the first row cast

# Get the director name
smd['directors'] = smd['crew'].apply(director_name)

#print(smd['cast'])
# Take the character names alone and make it as cast then take the first three cast members
smd['cast'] = smd['cast'].apply(lambda cast_list: [cast['name'] for cast in cast_list] if isinstance(cast_list, list) else []) # If list
smd['cast'] = smd['cast'].apply(lambda cast_list: cast_list[:3] if len(cast_list) >= 3 else cast_list)
#print(smd['cast'])

# Same for keywords
smd['keywords'] = smd['keywords'].apply(lambda keywords_list: [ keywords['name'] for keywords in keywords_list] if isinstance(keywords_list, list) else [])
#print(smd['keywords'])

# Process director and cast names to lower case and no space
smd['cast'] = smd['cast'].apply(lambda cast_names: [str.lower(name.replace(" ","")) for name in cast_names])
smd['directors'] = smd['directors'].astype('str').apply(lambda director_name: str.lower(director_name.replace(" ","")))
#print(smd['cast'])
#print(smd['directors'])

# Make directors into three stacked list in order to match the cast members
smd['directors'] = smd['directors'].apply(lambda name: [name, name, name])
#print(smd['directors'][0])

# Take the most important keywords corpus sepeartely (for each movie [movieId, keyword])
#smd.apply(lambda movie_data: print(movie_data['keywords']), axis = 1)
keywords_movies = smd.apply(lambda movie_data: pd.Series(movie_data['keywords']), axis = 1).stack().reset_index(level = 1, drop = True)
keywords_movies.name = 'keyword'
#print(keywords_movies)

# Remove records having less keywords
keywords_movies = keywords_movies.value_counts()
#print(keywords_movies)
keywords_movies = keywords_movies[keywords_movies > 1]
#print(keywords_movies)


stemmer = SnowballStemmer('english')

# Process most important keywords to lower case stem words with no space
smd['keywords'] = smd['keywords'].apply(lambda keywords: keep_most_important_keywords(keywords, keywords_movies))
smd['keywords'] = smd['keywords'].apply(lambda keywords: [stemmer.stem(keyword) for keyword in keywords])
keywords_dataset = smd
smd['keywords'] = smd['keywords'].apply(lambda keywords: [str.lower(keyword.replace(" ","")) for keyword in keywords])
#print(smd['keywords'])

# Combining all the key words directors, geners, cast, crew etc to get a string of words
smd['soup'] = smd['keywords'] + smd['cast'] + smd['directors'] + smd['genres']
smd['soup'] = smd['soup'].apply(lambda x: " ".join(x))
#print(smd['soup'])

# Get the count for each "soup"
count = CountVectorizer(analyzer = 'word', ngram_range = (1,2), min_df = 0, stop_words = 'english')
count_mat = count.fit_transform(smd['soup'])
cos_sim = cosine_similarity(count_mat, count_mat)

titles = smd['title']

smd = smd.reset_index()
indices = pd.Series(smd.index, index = smd['title'])    # A map between titles and movies index
#print(indices)

def get_recommendations_details(movie_name):
    idx = indices[movie_name]
    print(movie_name)
    print(idx)
    sim_score = list(enumerate(cos_sim[idx]))
    sim_score = sorted(sim_score, key = lambda id_score: id_score[1], reverse = True)
    sim_score = sim_score[1:31]

    #movies_idx = [print(id_score) for id_score in sim_score]
    movies_idx = [id_score[0] for id_score in sim_score]
    movies_details = smd.iloc[movies_idx][['title', 'vote_average', 'vote_count', 'year', 'imdb_id']]
    #print(movies_details)

    vote_count = movies_details[movies_details['vote_count'].notnull()]['vote_count'].astype('int')
    vote_averages = movies_details[movies_details['vote_average'].notnull()]['vote_average'].astype('int')

    vote_average = vote_averages.mean()         # Votes average
    min_votes = vote_count.quantile(0.60)       # Minimum votes required, over 60%

    quantifi = movies_details[movies_details['vote_count'] >= min_votes]
    quantifi['vote_count'] = quantifi['vote_count'].astype('int')
    quantifi['vote_average'] = quantifi['vote_average'].astype('int')

    quantifi['weight_rating'] = quantifi.apply(lambda movie_data: weight_rating(movie_data, min_votes, vote_average), axis = 1)
    quantifi = quantifi.sort_values('weight_rating', ascending = False).head(10)
    return quantifi



#------
# Keywords recommender
gathered_md = description_dataset.merge(keywords_dataset, on = 'id')

# Should be run only once, for the first time
# import nltk
# nltk.download('stopwords')

from nltk.corpus import stopwords
stop_words = set(stopwords.words('english'))

def getWordList(x):
    rough_wordList = re.sub("[^\w]", " ",  x).split()
    wordList = []
    for word in rough_wordList:
        if word not in stop_words:
            wordList.append(word)

    return wordList

import gensim

gathered_md['dataset'] = gathered_md['description'].apply(lambda description: getWordList(description))
gathered_md['dataset'] = gathered_md['dataset'] + gathered_md['keywords']

# Prepare the bow_corpus
words_for_dictionary = gathered_md['dataset'].tolist()
dictionary = gensim.corpora.Dictionary(words_for_dictionary)
bow_corpus = [dictionary.doc2bow(doc) for doc in words_for_dictionary]

# Transform bow_corpus in a tf-idf vector
tfidf = models.TfidfModel(bow_corpus) 
corpus_tfidf = tfidf[bow_corpus]

lsi = models.LsiModel(corpus = corpus_tfidf, id2word = dictionary, num_topics = 5)
# Compute a similarity matrix, which it's neccessary later, for query
indexList = similarities.MatrixSimilarity(lsi[corpus_tfidf])

def get_recommendations_keywords(query):
    # Transform in a bow corpus the query
    vec_bow = dictionary.doc2bow(getWordList(query))
    # Convert the query to LSI space
    vec_lsi = lsi[vec_bow]  
    
    # Perform a similarity query against the corpus
    sims = indexList[vec_lsi]  

    sims = sorted(enumerate(sims), key=lambda item: -item[1])
    
    # Retrieve top 10 results
    movies_idx = [id_score[0] for id_score in sims[:10]]
    movies_details = gathered_md.iloc[movies_idx][['title_x', 'imdb_id_x']]
    movies_details.rename(columns={"title_x": "title", "imdb_id_x": "imdb_id"}, inplace=True)
    return movies_details


#------
#User recommender
#movie_list = ['The Lion King', 'The Dark Knight Rises', 'The Godfather', 'Tarzan', 'Apocalypse Now']

def get_recommendations_wishlist(movies_wishlist):
    # create a copy of the movie dataframe and add a column in which we aggregated the scores
    user_scores = pd.DataFrame(smd['title'])
    user_scores['weight_rating'] = 0.0

    for movie_name in movies_wishlist:
        top_titles_df = get_recommendations_details(movie_name)
        #print(top_titles_df)
        #print(user_scores)
        # aggregate the scores
        user_scores = pd.concat([user_scores, top_titles_df[['title', 'weight_rating', 'imdb_id']]]).groupby(['title'], as_index=False).sum({'weight_rating'})
        #print(user_scores)
    # sort and print the aggregated scores
    user_scores = user_scores.sort_values('weight_rating', ascending=False)[1:21]
    #print(user_scores)
    return user_scores