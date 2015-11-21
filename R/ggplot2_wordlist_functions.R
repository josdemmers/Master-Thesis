# load packages
#install.packages("RCurl")
library(RCurl)
library(XML)

# download html
html <- getURL("http://docs.ggplot2.org/current/", followlocation = TRUE)

# parse html
doc = htmlParse(html, asText=TRUE)
wordlist <- xpathSApply(doc, "//code//a", xmlValue)
wordlist
transform(wordlist)
write.csv(wordlist, file="ggplot2_wordlist.csv", row.names = FALSE, fileEncoding = "UTF-8")
