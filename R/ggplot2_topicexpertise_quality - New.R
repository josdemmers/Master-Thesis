library('ggplot2')
library('reshape2')  # this is the library that lets you flatten out data

#Stack Overflow#
#TFIDF#

dfa <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_TFIDF_avg_rank_1.csv',sep=';',dec=',')
dfb <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_TFIDF_avg_rank_2.csv',sep=';',dec=',')
dfc <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_TFIDF_avg_rank_3.csv',sep=';',dec=',')
dfd <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_TFIDF_avg_rank_4.csv',sep=';',dec=',')
dfe <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_TFIDF_avg_rank_5.csv',sep=';',dec=',')
dff <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_TFIDF_avg_rank_6.csv',sep=';',dec=',')
dfg <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_TFIDF_avg_rank_7.csv',sep=';',dec=',')
dfh <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_TFIDF_avg_rank_8.csv',sep=';',dec=',')
dfi <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_TFIDF_avg_rank_9.csv',sep=';',dec=',')
dfj <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_TFIDF_avg_rank_10.csv',sep=';',dec=',')

bucket<-list(a=dfa$TFIDF_avg_rank,b=dfb$TFIDF_avg_rank,c=dfc$TFIDF_avg_rank,d=dfd$TFIDF_avg_rank,e=dfe$TFIDF_avg_rank,
             f=dff$TFIDF_avg_rank,g=dfg$TFIDF_avg_rank,h=dfh$TFIDF_avg_rank,i=dfi$TFIDF_avg_rank,j=dfj$TFIDF_avg_rank)

# the melt command flattens the 'bucket' list into value/vectorname pairs
# the 2 columns are called 'value' and 'L1' by default
# 'fill' will color bars differently depending on L1 group
ggplot(melt(bucket), aes(value, fill = L1)) +
geom_histogram(position = "stack", binwidth=2) +
xlim(0,50) + ylim(0,1000) + xlab("Rank") + ylab("Count")

#Stack Overflow
#Custom#

df2a <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_Custom_avg_rank_1.csv',sep=';',dec=',')
df2b <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_Custom_avg_rank_2.csv',sep=';',dec=',')
df2c <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_Custom_avg_rank_3.csv',sep=';',dec=',')
df2d <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_Custom_avg_rank_4.csv',sep=';',dec=',')
df2e <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_Custom_avg_rank_5.csv',sep=';',dec=',')
df2f <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_Custom_avg_rank_6.csv',sep=';',dec=',')
df2g <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_Custom_avg_rank_7.csv',sep=';',dec=',')
df2h <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_Custom_avg_rank_8.csv',sep=';',dec=',')
df2i <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_Custom_avg_rank_9.csv',sep=';',dec=',')
df2j <- read.csv('F:\\!Laptop\\Master Thesis\\data\\SO_Custom_avg_rank_10.csv',sep=';',dec=',')

bucket<-list(a=df2a$Custom_avg_rank,b=df2b$Custom_avg_rank,c=df2c$Custom_avg_rank,d=df2d$Custom_avg_rank,e=df2e$Custom_avg_rank,
             f=df2f$Custom_avg_rank,g=df2g$Custom_avg_rank,h=df2h$Custom_avg_rank,i=df2i$Custom_avg_rank,j=df2j$Custom_avg_rank)

# the melt command flattens the 'bucket' list into value/vectorname pairs
# the 2 columns are called 'value' and 'L1' by default
# 'fill' will color bars differently depending on L1 group
ggplot(melt(bucket), aes(value, fill = L1)) +
geom_histogram(position = "stack", binwidth=2) +
xlim(0,50) + ylim(0,1000) + xlab("Rank") + ylab("Count")

#Google Groups#
#TFIDF#

dfa <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_TFIDF_avg_rank_1.csv',sep=';',dec=',')
dfb <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_TFIDF_avg_rank_2.csv',sep=';',dec=',')
dfc <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_TFIDF_avg_rank_3.csv',sep=';',dec=',')
dfd <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_TFIDF_avg_rank_4.csv',sep=';',dec=',')
dfe <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_TFIDF_avg_rank_5.csv',sep=';',dec=',')
dff <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_TFIDF_avg_rank_6.csv',sep=';',dec=',')
dfg <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_TFIDF_avg_rank_7.csv',sep=';',dec=',')
dfh <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_TFIDF_avg_rank_8.csv',sep=';',dec=',')
dfi <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_TFIDF_avg_rank_9.csv',sep=';',dec=',')
dfj <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_TFIDF_avg_rank_10.csv',sep=';',dec=',')

bucket<-list(a=dfa$TFIDF_avg_rank,b=dfb$TFIDF_avg_rank,c=dfc$TFIDF_avg_rank,d=dfd$TFIDF_avg_rank,e=dfe$TFIDF_avg_rank,
             f=dff$TFIDF_avg_rank,g=dfg$TFIDF_avg_rank,h=dfh$TFIDF_avg_rank,i=dfi$TFIDF_avg_rank,j=dfj$TFIDF_avg_rank)

# the melt command flattens the 'bucket' list into value/vectorname pairs
# the 2 columns are called 'value' and 'L1' by default
# 'fill' will color bars differently depending on L1 group
ggplot(melt(bucket), aes(value, fill = L1)) +
  geom_histogram(position = "stack", binwidth=2) +
  xlim(0,50) + ylim(0,1000) + xlab("Rank") + ylab("Count")

#Google Groups
#Custom#

df2a <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_Custom_avg_rank_1.csv',sep=';',dec=',')
df2b <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_Custom_avg_rank_2.csv',sep=';',dec=',')
df2c <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_Custom_avg_rank_3.csv',sep=';',dec=',')
df2d <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_Custom_avg_rank_4.csv',sep=';',dec=',')
df2e <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_Custom_avg_rank_5.csv',sep=';',dec=',')
df2f <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_Custom_avg_rank_6.csv',sep=';',dec=',')
df2g <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_Custom_avg_rank_7.csv',sep=';',dec=',')
df2h <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_Custom_avg_rank_8.csv',sep=';',dec=',')
df2i <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_Custom_avg_rank_9.csv',sep=';',dec=',')
df2j <- read.csv('F:\\!Laptop\\Master Thesis\\data\\GG_Custom_avg_rank_10.csv',sep=';',dec=',')

bucket<-list(a=df2a$Custom_avg_rank,b=df2b$Custom_avg_rank,c=df2c$Custom_avg_rank,d=df2d$Custom_avg_rank,e=df2e$Custom_avg_rank,
             f=df2f$Custom_avg_rank,g=df2g$Custom_avg_rank,h=df2h$Custom_avg_rank,i=df2i$Custom_avg_rank,j=df2j$Custom_avg_rank)

# the melt command flattens the 'bucket' list into value/vectorname pairs
# the 2 columns are called 'value' and 'L1' by default
# 'fill' will color bars differently depending on L1 group
ggplot(melt(bucket), aes(value, fill = L1)) +
  geom_histogram(position = "stack", binwidth=2) +
  xlim(0,50) + ylim(0,1000) + xlab("Rank") + ylab("Count")
