#install.packages("ggplot2")
library('ggplot2')

df <- read.csv('C:\\Users\\Uthar\\Dropbox\\TUE\\Master Thesis\\Master Thesis\\R\\GG_stats.csv',sep=';')
df <- read.csv('C:\\Users\\s100026\\Dropbox\\TUE\\Master Thesis\\Master Thesis\\R\\GG_stats.csv',sep=';')
df$fitems<-cut(df$Answers+df$Questions,breaks=c(0,1,10,25,100,2000),labels=c("1","2-10","11-25","26-100", "101-2000"))
ggplot(data=df, aes(df$fitems)) +
  geom_bar(col="red") +labs(y="Count",x="Posts") +
  labs(title="Post count frequency users") +
  theme(plot.title=element_text(face="bold"))