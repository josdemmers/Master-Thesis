#install.packages("XML") #xmlParse
library(XML)
#install.packages("tm") #Corpus
library(tm)

##Google Groups##
doc <- xmlParse("F:\\!Laptop\\Master Thesis\\data\\Google Groups\\Output\\GG_topics.xml", asText=FALSE)
messages <- xpathSApply(doc, "//Message//content", xmlValue)

##Stack Overflow
doc <- xmlParse("F:\\!Laptop\\Master Thesis\\data\\Stack Overflow\\SO_Posts_filter.xml", asText=FALSE)
messages <- xpathSApply(doc, "//row", xmlGetAttr, 'Body')

#Make corpus for text mining
vector <- VectorSource(messages)
corpus <- Corpus(vector)

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

#TF-IDF
corpus <- tm_map(corpus, tolower)
#corpus <- tm_map(corpus, removePunctuation, preserve_intra_word_dashes = TRUE)
#corpus <- tm_map(corpus, removeNumbers)
corpus <- tm_map(corpus, replaceExpressions)
corpus <- tm_map(corpus, replaceExpressions)#Repeat to replace remaining digits.
#corpus <- tm_map(corpus, stemDocument, language="english")
corpus <- tm_map(corpus, removeWords, stopwords("english"))
corpus <- tm_map(corpus, removeWords, stopwords("SMART"))
corpus <- tm_map(corpus, stripWhitespace)
corpus <- tm_map(corpus, PlainTextDocument)
terms <-DocumentTermMatrix(corpus, control = list(weighting = function(x) weightTfIdf(x, normalize = FALSE)))

##Google Groups##
#matrix <- inspect(terms)
#nrow:nrow,ncol:ncol
#1:18341,1:39003
#matrix <- inspect(terms[18001:18341,1:39003])
#df <- as.data.frame(matrix, stringsAsFactors = FALSE)
#write.csv(df, file="TF-IDF_GG\\GG_wordlist_tfidf18001.csv", row.names = FALSE)

##Stack Overflow
#matrix <- inspect(terms)
#nrow:nrow,ncol:ncol
#1:14054,1:48444
matrix <- inspect(terms[13001:14054,1:48444])
df <- as.data.frame(matrix, stringsAsFactors = FALSE)
write.csv(df, file="TF-IDF_SO\\SO_wordlist_tfidf13001_test.csv", row.names = FALSE)