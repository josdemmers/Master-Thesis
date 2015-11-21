library('ggplot2')

#TFIDF#

df <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_TFIDF_avg_rank_10 - freq.csv',sep=';',dec=',')
df <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_TFIDF_avg_rank_10.csv',sep=';',dec=',')
df <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_TFIDF_avg_rank_10 - relevant.csv',sep=';',dec=',')

df <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_TFIDF_avg_rank_10.csv',sep=';',dec=',')

df$TFIDF_avg_rank
median(df$TFIDF_avg_rank)
mean(df$TFIDF_avg_rank)

TFIDF_avg_rank <- df$TFIDF_avg_rank
bins <- seq(-0.1,50.0,by=10)
ranks <- cut(TFIDF_avg_rank, bins)
table(ranks)
transform(table(ranks))

hist(TFIDF_avg_rank, main = "", xlab = "Avg Ranking", ylim = c(0, 200))

#Custom#

df2 <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_Custom_avg_rank_10 - nofreq.csv',sep=';',dec=',')
df2 <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_Custom_avg_rank_2.csv',sep=';',dec=',')

df2 <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_Custom_avg_rank_10.csv',sep=';',dec=',')

df2$Custom_avg_rank
median(df2$Custom_avg_rank)
mean(df2$Custom_avg_rank)

Custom_avg_rank <- df2$Custom_avg_rank
bins2 <- seq(-0.1,50.0,by=10)
ranks2 <- cut(Custom_avg_rank, bins2)
table(ranks2)
transform(table(ranks2))

hist(Custom_avg_rank, main = "", xlab = "Avg Ranking", ylim = c(0, 80))
