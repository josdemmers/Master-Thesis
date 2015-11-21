#### Correlation in R
#### Read in the data

#### OLD DATA ####
#data1 <- read.csv("C:\\Users\\s100026\\Dropbox\\TUE\\Master Thesis\\Master Thesis\\R\\GG_stats_correlation - OLD.csv", header = TRUE,sep = ";")
#data1 <- read.csv("C:\\Users\\s100026\\Dropbox\\TUE\\Master Thesis\\Master Thesis\\R\\GG_stats_correlation - Top50 - OLD.csv", header = TRUE,sep = ";")

#### GG ####
data1 <- read.csv("C:\\Users\\s100026\\Dropbox\\TUE\\Master Thesis\\Master Thesis\\R\\GG_stats_correlation.csv", header = TRUE,sep = ";")
data1 <- read.csv("C:\\Users\\s100026\\Dropbox\\TUE\\Master Thesis\\Master Thesis\\R\\GG_stats_correlation - Top50.csv", header = TRUE,sep = ";")
data1 <- read.csv("C:\\Users\\Uthar\\Dropbox\\TUE\\Master Thesis\\Master Thesis\\R\\GG_stats_correlation.csv", header = TRUE,sep = ";")
data1 <- read.csv("C:\\Users\\Uthar\\Dropbox\\TUE\\Master Thesis\\Master Thesis\\R\\GG_stats_correlation - Top50.csv", header = TRUE,sep = ";")

#### SO ####
data1 <- read.csv("C:\\Users\\s100026\\Dropbox\\TUE\\Master Thesis\\Master Thesis\\R\\SO_stats_correlation.csv", header = TRUE,sep = ";")
data1 <- read.csv("C:\\Users\\s100026\\Dropbox\\TUE\\Master Thesis\\Master Thesis\\R\\SO_stats_correlation - Top50.csv", header = TRUE,sep = ";")
data1 <- read.csv("C:\\Users\\Uthar\\Dropbox\\TUE\\Master Thesis\\Master Thesis\\R\\SO_stats_correlation.csv", header = TRUE,sep = ";")
data1 <- read.csv("C:\\Users\\Uthar\\Dropbox\\TUE\\Master Thesis\\Master Thesis\\R\\SO_stats_correlation - Top50.csv", header = TRUE,sep = ";")

##TODO data.frame row selection?


data1$rank <- c(1:nrow(data1))#Add rank (Integer from 1 to size of data1)
all <- unique(sort(c(data1$ZScore,data1$ZDegree,data1$ExpertiseRank))) #list of all user ids; c combines data;
zscores <- c()
zdegrees <- c()
expertiseranks <- c()
for (user in all) {
  zscores <- c(zscores,as.numeric(subset(data1, ZScore==user)["rank"]))
  zdegrees <- c(zdegrees,as.numeric(subset(data1, ZDegree==user)["rank"]))
  expertiseranks <- c(expertiseranks,as.numeric(subset(data1, ExpertiseRank==user)["rank"]))
}

#Indexes in df are sorted IDs, stats in other columns are the rankings of each users by an algorithm  
df <- data.frame(all,zscores,zdegrees,expertiseranks)
names(df) <- c("UserID","RankAccZScore","RankAccZDegree","RankAccExpertiseRank")
names(df) <- c("User IDs","ZScore","ZDegree","ExpertiseRank")
#### Create a scatterplot
plot(df$RankAccZScore,df$RankAccZDegree)
plot(df$RankAccZScore,df$RankAccExpertiseRank)
plot(df$RankAccZDegree,df$RankAccExpertiseRank)
#### Create a mtrix plot of scatterplots
pairs(df)
#### Calculate the correlation coefficient
cor(df)#Default is spearman
cor(df, method = "spearman")
cor(df, method = "kendall")
