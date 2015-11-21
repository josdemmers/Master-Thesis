#install.packages("XML") #xmlParse
library(XML)
#install.packages("tm") #Corpus
library(tm)

##Google Groups##
doc <- xmlParse("F:\\!Laptop\\Master Thesis\\data\\Google Groups - Test\\Output\\GG_topics.xml", asText=FALSE)
doc <- xmlParse("F:\\!Laptop\\Master Thesis\\data\\Google Groups\\Output\\GG_topics.xml", asText=FALSE)
userlist <- xpathSApply(doc, "//Message/User/Name", xmlValue)
topicidlist <- xpathSApply(doc, "//Topic/ID", xmlValue)
questions <- xpathSApply(doc, "//Messages//Message[1]//content", xmlValue)

##Stack Overflow##
doc <- xmlParse("F:\\!Laptop\\Master Thesis\\data\\Stack Overflow\\SO_Posts_filter.xml", asText=FALSE)
userlist <- xpathSApply(doc, "//row", xmlGetAttr, 'OwnerUserId')
topicidlist <- xpathSApply(doc, "//row[@PostTypeId='1']", xmlGetAttr, 'Id')
questions <- xpathSApply(doc, "//row[@PostTypeId='1']", xmlGetAttr, 'Body')

##GitHub##
#doc <- xmlParse("F:\\!Laptop\\Master Thesis\\data\\GitHub\\GH_posts.xml", asText=FALSE)
#userlist <- xpathSApply(doc, "//Message", xmlGetAttr, "UserId")
#topicidlist <- xpathSApply(doc, "//Topic", xmlGetAttr, "Id")
#questions <- xpathSApply(doc, "//Messages//Message[1]", xmlGetAttr, "Content")

#Make corpus for text mining
vector <- VectorSource(questions)
corpus <- Corpus(vector)

# process text
replaceExpressions <- function(x) UseMethod("replaceExpressions", x)

replaceExpressions.PlainTextDocument <- replaceExpressions.character  <- function(x) {
  x <- gsub("[^[:alnum:]._]", " ", x) #Replaces all non alphabetic/numeric, dot, and underscore chars by space 
  x <- gsub("\\d+\\.\\d+", " ", x) #Replaces doubles by space
  x <- gsub("\\.+[^a-z0-9]", " ", x) #Replaces dots at end of string by space.
  x <- gsub(" [[:punct:]]+", " ", x) #Replaces punctiation at start of string by space.
  x <- gsub("\\d+[a-z]+", " ", x) #Replaces string containing numbers follow by letters by space.
  x <- gsub(" \\d+", " ", x) #Replaces numeric string by space, i.e., 10 is replaced by log10 is not.
  
  return(x)
}

a <- tm_map(corpus, tolower)
#a <- tm_map(a, stemDocument, language="english")
a <- tm_map(a, removeWords, stopwords("english"))
a <- tm_map(a, removeWords, stopwords("SMART"))
#a <- tm_map(a, removePunctuation, preserve_intra_word_dashes = TRUE)
a <- tm_map(a, replaceExpressions)
a <- tm_map(a, replaceExpressions)
#a <- tm_map(a, removeNumbers)
a <- tm_map(a, stripWhitespace)

#a <- tm_map(a, PlainTextDocument)
m = as.matrix(t(a))

##Google Groups##
#Manually check output to make sure each question is on a separate line
#write.csv(m, file="GG_questionlist.csv", row.names = FALSE)
#write.csv(topicidlist, file="GG_topicIDlist.csv", row.names = FALSE)
#write.csv(userlist, file="GG_userlist.csv", row.names = FALSE, fileEncoding = "UTF-8")

##Stack Overflow##
#Manually check output to make sure each question is on a separate line
write.csv(m, file="SO_questionlist.csv", row.names = FALSE)
write.csv(topicidlist, file="SO_topicIDlist.csv", row.names = FALSE)
write.csv(userlist, file="SO_userlist.csv", row.names = FALSE, fileEncoding = "UTF-8")

##GitHub##
#Manually check output to make sure each question is on a separate line
#write.csv(m, file="GH_issuelist.csv", row.names = FALSE)
#write.csv(topicidlist, file="GH_topicIDlist.csv", row.names = FALSE)
#write.csv(userlist, file="GH_userlist.csv", row.names = FALSE, fileEncoding = "UTF-8")