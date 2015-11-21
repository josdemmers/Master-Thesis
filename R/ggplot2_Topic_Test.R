#install.packages("SnowballC")
library(SnowballC)
#install.packages("XML") #xmlParse
library(XML)
#install.packages("tm") #Corpus
library(tm)
library(slam) #row_sums
#install.packages("lda") #LDA??
#library(lda)
#install.packages("MASS") #LDA??
#library(MASS)
#install.packages("wordcloud")
library(wordcloud)
#install.packages("RColorBrewer")
library(RColorBrewer)

doc <- xmlParse("http://ieeexplore.ieee.org/gateway/ipsSearch.jsp?issn=1063-6773&hc=1001&rs=1000", asText=FALSE)
doc <- xmlParse("D:\\Desktop\\ipsSearch.jsp.xml", asText=FALSE)

#### extracted their abstracts
abstracts <- getNodeSet(doc,"//document//abstract")
abstracts <- xpathSApply(doc, "//document//abstract", xmlValue)
#### created the corpus and the document-term-matrix
vector <- VectorSource(abstracts)
corpus <- Corpus(vector)
icsm_dtm <- DocumentTermMatrix(corpus, control = list(stemming = TRUE, stopwords = TRUE, minWordLength = 3, removeNumbers = TRUE, removePunctuation = TRUE))
#which turned out to be quite big
dim(icsm_dtm)
#so we have calculated TF-IDF and reduced the matrix based on it (the threshold is taken from the paper above)
term_tfidf <- tapply(icsm_dtm$v/row_sums(icsm_dtm)[icsm_dtm$i], icsm_dtm$j, mean) * log2(nDocs(icsm_dtm)/col_sums(icsm_dtm > 0))
icsm_dtm <- icsm_dtm[, term_tfidf >= 0.1]
icsm_dtm <- icsm_dtm[row_sums(icsm_dtm) > 0,]
dim(icsm_dtm)
#The next step is the actual extraction of the topics. I've decided to extract 30 and to use 2010 as the random #number seed. The values are from the paper and I have no clue how one should choose them.
# <!-- Stops working here -->
icsm_TM <- list(VEM = LDA(icsm_dtm, k = k, control = list(seed = SEED)),VEM_fixed = LDA(icsm_dtm, k = k, control = list(estimate.alpha = FALSE, seed = SEED)), Gibbs = LDA(icsm_dtm, k = k, method = "Gibbs", control = list(seed = SEED, burnin = 1000, thin = 100, iter = 1000)), CTM = CTM(icsm_dtm, k = k, control = list(seed = SEED, var = list(tol = 10^-4), em = list(tol = 10^-3))))
#Finally, I've looked at the topics found. Topic is the vector of the most important topics per document










### TEST 2 ###
#doc <- xmlParse("D:\\Desktop\\GG_topics.xml", asText=FALSE)
doc <- xmlParse("F:\\!Laptop\\Master Thesis\\data\\Google Groups\\Output\\GG_topics.xml", asText=FALSE)
messages <- xpathSApply(doc, "//Message//content", xmlValue)
#messages <- xpathSApply(doc, "//Message[User/Name = 'Ben']/content", xmlValue)
#messages <- xpathSApply(doc, "//Message[User/Name = 'Brandon Hurr']/content", xmlValue)
messages <- xpathSApply(doc, "//Message[User/Name = 'Dennis Murphy']/content", xmlValue)
userlist <- xpathSApply(doc, "//Message/User/Name", xmlValue)
topicidlist <- xpathSApply(doc, "//Topic/ID", xmlValue)
#questions <- xpathSApply(doc, "//Messages//Message[1]//content", xmlValue)  
#Make corpus for text mining
vector <- VectorSource(messages)
corpus <- Corpus(vector)

# process text...
#skipWords <- function(x) removeWords(x, stopwords("english"))
#skipWords <- function(x) removeWords(x, stopwords("SMART"))
#funcs <- list(tolower, removePunctuation, removeNumbers, stripWhitespace, skipWords)
#funcs <- list(tolower, removeNumbers, stripWhitespace, skipWords)
#a <- tm_map(a, PlainTextDocument)
#a <- tm_map(corpus, FUN = tm_reduce, tmFuns = funcs)

replaceExpressions <- function(x) UseMethod("replaceExpressions", x)

replaceExpressions.PlainTextDocument <- replaceExpressions.character  <- function(x) {
  #x <- gsub(".", " ", x, ignore.case =FALSE, fixed = TRUE)
  #x <- gsub('^[[:punct:]'"":±</>]+$',"", x, perl=TRUE)
  #x <- gsub('^[[:punct:]'"":±</>]+$',"", x, fixed = TRUE)
  #x <- gsub(",", " ", x, ignore.case =FALSE, fixed = TRUE)
  #x <- gsub(":", " ", x, ignore.case =FALSE, fixed = TRUE)
  #x <- gsub("(", " ", x, ignore.case =FALSE, fixed = TRUE)
  #x <- gsub(")", " ", x, ignore.case =FALSE, fixed = TRUE)
  #x <- gsub("=", " ", x, ignore.case =FALSE, fixed = TRUE)
  #x <- gsub("\\", " ", x, ignore.case =FALSE, fixed = TRUE)
  #x <- gsub("/", " ", x, ignore.case =FALSE, fixed = TRUE)
  #x <- gsub("[_]+$", " ", x, ignore.case =FALSE, fixed = TRUE)
  #x <- gsub("[[:punct:]^_]", " ", x, ignore.case =FALSE, fixed = FALSE)
  #todo strings containing @ using regex
  #x <- gsub('^[[:punct:]]+$',"", x, fixed = TRUE)
  #x <- gsub('^[[:punct:]]+',"", x, fixed = TRUE)
  #x <- gsub('[[:punct:]]+$',"", x, fixed = TRUE)
  #x <- gsub("(?!_)[[:punct:]]|(?!.)[[:punct:]]", " ", x, perl=TRUE)
  #x <- gsub("(?!_)[[:punct:]]", " ", x, perl=TRUE)
  #x <- gsub("[^[:alnum:][:space:]'._]", " ", x)
  x <- gsub("[^[:alnum:]._]", " ", x) #Replaces all non alphabetic/numeric, dot, and underscore chars by space 
  x <- gsub("\\d+\\.\\d+", " ", x) #Replaces doubles by space
  x <- gsub("\\.+[^a-z0-9]", " ", x) #Replaces dots at end of string by space.
  x <- gsub(" [[:punct:]]+", " ", x) #Replaces punctiation at start of string by space.
  x <- gsub("\\d+[a-z]+", " ", x) #Replaces string containing numbers follow by letters by space.
  x <- gsub(" \\d+", " ", x) #Replaces numeric string by space, i.e., 10 is replaced by log10 is not.
  x <- gsub("dennis", " ",x)
  
  return(x)
}

a <- tm_map(corpus, tolower)
#a <- tm_map(a, removePunctuation, preserve_intra_word_dashes = TRUE)
a <- tm_map(a, replaceExpressions)
a <- tm_map(a, replaceExpressions)
#a <- tm_map(a, removeNumbers)
a <- tm_map(a, stripWhitespace)
#a <- tm_map(a, stemDocument, language="english")
a <- tm_map(a, removeWords, stopwords("english"))
a <- tm_map(a, removeWords, stopwords("SMART"))

a <- tm_map(a, PlainTextDocument)

#a.dtm1 <- TermDocumentMatrix(a, control = list(wordLengths = c(3,10))) 
#a.dtm1 <- TermDocumentMatrix(a, control = list(wordLengths = c(3,20))) 
a.dtm1 <- TermDocumentMatrix(a, control = list(wordLengths = c(4,10))) 
# get most frequent words
#newstopwords <- findFreqTerms(a.dtm1, lowfreq=50)
newstopwords <- findFreqTerms(a.dtm1, lowfreq=20)
# remove most frequent words for this corpus
a.dtm2 <- a.dtm1[!(a.dtm1$dimnames$Terms) %in% newstopwords,] 
inspect(a.dtm2)

# carry on with typical things that can now be done, ie. cluster analysis
a.dtm3 <- removeSparseTerms(a.dtm2, sparse=0.7)
a.dtm3 <- removeSparseTerms(a.dtm2, sparse=1.0)
a.dtm.df <- as.data.frame(inspect(a.dtm3))
a.dtm.df.scale <- scale(a.dtm.df)
d <- dist(a.dtm.df.scale, method = "euclidean") 
fit <- hclust(d, method="ward")
plot(fit)

m = as.matrix(t(a.dtm1))
#m = as.matrix(t(a.dtm2))
# get word counts in decreasing order
word_freqs = sort(colSums(m), decreasing=TRUE) 
# create a data frame with words and their frequencies
dm = data.frame(word=names(word_freqs), freq=word_freqs)
# plot wordcloud
wordcloud(dm$word, dm$freq, random.order=FALSE, colors=brewer.pal(8, "Dark2"))







#TF-IDF
corpus2 <- corpus
corpus2 <- tm_map(corpus2, tolower)
#corpus2 <- tm_map(corpus2, removePunctuation, preserve_intra_word_dashes = TRUE)
#corpus2 <- tm_map(corpus2, removeNumbers)
corpus2 <- tm_map(corpus2, replaceExpressions)
corpus2 <- tm_map(corpus2, replaceExpressions)#Repeat to replace remaining digits.
corpus2 <- tm_map(corpus2, stripWhitespace)
#corpus2 <- tm_map(corpus2, stemDocument, language="english")
corpus2 <- tm_map(corpus2, removeWords, stopwords("english"))
corpus2 <- tm_map(corpus2, removeWords, stopwords("SMART"))
terms <-DocumentTermMatrix(corpus2, control = list(weighting = function(x) weightTfIdf(x, normalize = FALSE)))
#inspect(terms[1:10,700:800])
#inspect(terms[1:1,1:7493])


#matrix <- inspect(terms)
matrix <- inspect(terms[18001:18341,1:39003])
df <- as.data.frame(matrix, stringsAsFactors = FALSE)
write.csv(df, file="TF-IDF_GG\\messages18001.csv", row.names = FALSE)

write.csv(userlist, file="userlist.csv", row.names = FALSE, fileEncoding = "UTF-8");






##################################
# initialize a storage variable for Twitter tweets
mydata.vectors <- character(0)

# paginate to get more tweets
for (page in c(1:15))
{
  # search parameter
  twitter_q <- URLencode('#prolife OR #prochoice')
  # construct a URL
  twitter_url = paste('http://search.twitter.com/search.atom?q=',twitter_q,'&rpp=100&page=', page, sep='')
  # fetch remote URL and parse
  mydata.xml <- xmlParseDoc(twitter_url, asText=F)
  # extract the titles
  mydata.vector <- xpathSApply(mydata.xml, '//s:entry/s:title', xmlValue, namespaces =c('s'='http://www.w3.org/2005/Atom'))
  # aggregate new tweets with previous tweets
  mydata.vectors <- c(mydata.vector, mydata.vectors)
}

# how many tweets did we get?
length(mydata.vectors)
