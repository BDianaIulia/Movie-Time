import PySimpleGUI as sg

import main

#define layout
layout1 = [[sg.Text('Instert the genre', size=(14,1)),sg.Input('',key='eGenre')],
           [sg.Button('Search')],
           [sg.Text(size=(80,30), key='-OUTPUT-GENRES-')]]
layout2 = [[sg.Text('Instert the movie title', size=(17,1)),sg.Input('',key='eTitle2')],
           [sg.Button('Search')],
           [sg.Text(size=(80,30), key='-OUTPUT-TITLE-2-')]]
layout3 = [[sg.Text('Instert the movie title', size=(17,1)),sg.Input('',key='eTitle3')],
           [sg.Button('Search')],
           [sg.Text(size=(80,30), key='-OUTPUT-TITLE-3-')]]
layout4 = [[sg.Text('Instert the keyword', size=(17,1)),sg.Input('',key='eKeyWord')],
           [sg.Button('Search')],
           [sg.Text(size=(80,30), key='-OUTPUT-KEY-WORD-')]]
#Define Layout with Tabs         
tabgrp = [[sg.TabGroup([[sg.Tab('Top movies by genre', layout1, title_color='Red', border_width = 10, background_color='White',
                                tooltip='Top movies by genre', element_justification= 'center'),
                        sg.Tab('Movie Discription Based Recommender', layout2, title_color='Blue', background_color='White', element_justification= 'center'),
                        sg.Tab('Movie Destails Based Recommender', layout3, title_color='Black', background_color='White',
                           tooltip='Enter a movie title to get the list of recommendations', element_justification= 'center'),
                        sg.Tab('Keywords Recommender', layout4, title_color='Blue', background_color='White', element_justification= 'center')]], 
                        tab_location='centertop', title_color='White', tab_background_color='Dark Blue', selected_title_color='Blue',
                        selected_background_color='Gray', border_width=5)]]  
        
#Define Window
window = sg.Window("Tabs", tabgrp)

while True:
    event, values = window.read()
    print(values)
    # See if user wants to quit or window was closed
    if event == sg.WINDOW_CLOSED or event == 'Close':
        break
    # Output a message to the window
    if values['eGenre']:
        try:
            window['-OUTPUT-GENRES-'].update(main.top_movies(values['eGenre']))
            print(main.top_movies(values['eGenre']))
        except ValueError:
            window['-OUTPUT-GENRES-'].update('Whoops! Something went wrong.')

    if values['eTitle2']:
        try:
            window['-OUTPUT-TITLE-2-'].update(main.get_recommendations(values['eTitle2']).head(10))
            print(main.get_recommendations(values['eTitle2']).head(10))
        except ValueError:
            window['-OUTPUT-TITLE-2-'].update('Whoops! Something went wrong.')

    if values['eTitle3']:
        try:
            window['-OUTPUT-TITLE-3-'].update(main.recommender(values['eTitle3']))
            print(main.recommender(values['eTitle3']))
        except ValueError:
            window['-OUTPUT-TITLE-3-'].update('Whoops! Something went wrong.')

    if values['eKeyWord']:
        try:
            window['-OUTPUT-KEY-WORD-'].update(main.resultForQuery(values['eKeyWord']))
            print(main.resultForQuery(values['eKeyWord']))
        except ValueError:
            window['-OUTPUT-KEY-WORD-'].update('Whoops! Something went wrong.')

#access all the values and if selected add them to a string
window.close() 