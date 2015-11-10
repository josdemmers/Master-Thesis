#### Notice
This crawler uses the mobile pages of google groups to retrieve topic information. The required data is extracted by identifying the html elements that contain the data by their class name. However, those class names change once in a while. 
If the tool is not able to extract any information make sure the class names in the method readtopic() are still correct.

# google-groups-crawler
Google Groups Crawler written in C#

Instructions Crawler:
- Enter Group ID.
- Press Load List or File to load topic list.
- File topiclist.txt, containing topic ID and Title, is created when all topics are loaded.
- Press Read Topics. Additionally you can change the start index.

Instructions Parser:
- Choose your settings (Base64, Default/Activity/ZScore), read tooltip for more information.
- Press Import Data to read all topics from /Output/Topics/
- Press Export Data to create the users.xml and topics.xml files.

![Interface](http://oi59.tinypic.com/mt35o9.jpg)

# License
google-groups-crawler is licensed under [GPL v2](https://github.com/josdemmers/google-groups-crawler/blob/master/LICENSE)
