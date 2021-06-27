# Project: Personal Video Game Library
# Name: Estevan Cordero

## Summary
  - This project is a library that keeps track of a collection of video games by seperating it into 3 seperate tables.
  - Console Table - keeps track of the platform of the video game.
  - VideoGames Table - keeps track of a video game entry, and the content of it.
  - Session Table - keeps track of any sessions and messages that the user wish to leave for a picticular video game.

## Task
  - Made 4 tables to manage data.
  - Tables included having a M-M relationship(Console-VideoGames), and a O-M relationship(VideoGames-Session)
  - Included the feature to able to add an image to a video game entry.
  - Included CRUD to manage values on web pages. 
  
## Future Plans
  - Improving the css. 
  - New features  
    - Including a rating system for the video games.
    - Merging session and video games into one page instead of seperate pages.
    - Allowing users to login, and have seperate library.
       
## Know Bugs
  - Can't delete a video game entry.
  - When updating an image for a video game entry, the page needs to be refresh to see the change.
  - When making a new video game entry it will not save the message, but when updating it will assign it to that game.
  - Can't unassign a console to a video game when viewing it in details. 
