import flask 
from flask import request, jsonify
import main

app = flask.Flask(__name__)
app.config["DEBUG"] = True

@app.errorhandler(404)
def page_not_found(e):
    return "<h1>404</h1><p>The resource could not be found.</p>", 404

@app.route('/', methods=['GET'])
def healthCheck():
    return "Running"

@app.route('/api/genre-recommender', methods=['GET'])
def genre_recommendation():
    genre = request.args.get('genre')
    if genre is None:
        return page_not_found(404)
    try:
        recommendations = main.top_movies(genre)
        print(recommendations)
        return recommendations.to_json(orient="table"), 200
    except:
        return page_not_found(404)

@app.route('/api/movie-description-based-recommender', methods=['GET'])
def movie_title_descripiton_recommendation():
    movie_title = request.args.get('movie_title')
    if movie_title is None:
        return page_not_found(404)
    try:
        recommendations = main.get_recommendations_overview(movie_title)
        print(recommendations)
        return recommendations.to_json(orient="table"), 200
    except:
        return page_not_found(404)

@app.route('/api/movie-details-based-recommender', methods=['GET'])
def movie_title_details_recommendation():
    movie_title = request.args.get('movie_title')
    if movie_title is None:
        return page_not_found(404)
    try:
        recommendations = main.get_recommendations_details(movie_title)
        print(recommendations)
        return recommendations.to_json(orient="table"), 200
    except:
        return page_not_found(404)

@app.route('/api/movie-keywords-based-recommender', methods=['GET'])
def movie_keywords_recommendation():
    keywords = request.args.get('keywords')
    if keywords is None:
        return page_not_found(404)
    try:
        recommendations = main.get_recommendations_keywords(keywords)
        print(recommendations)
        return recommendations.to_json(orient="table"), 200
    except:
        return page_not_found(404)

@app.route('/api/movie-wishlist-based-recommender', methods=['POST'])
def movie_wishlist_recommendation():
    bodyJson = request.json
    if bodyJson is None:
        return page_not_found(404)
    wishlist = bodyJson.get('movie_titles')
    try:
        recommendations = main.get_recommendations_wishlist(wishlist)
        print(recommendations)
        return recommendations.to_json(orient="table"), 200
    except:
        return page_not_found(404)

app.run()