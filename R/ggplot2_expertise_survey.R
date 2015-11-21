df <- read.csv('C:\\Users\\Uthar\\Dropbox\\TUE\\Master Thesis\\Master Thesis\\R\\ggplot2_expertise_survey.csv',sep=';')
df <- read.csv('C:\\Users\\s100026\\Dropbox\\TUE\\Master Thesis\\Master Thesis\\R\\ggplot2_expertise_survey.csv',sep=';')

#names(df)
#Jean.Olivier.Irisson
#Ben                  
#Dennis.Murphy
#Roman.Luštrik        
#Brandon.Hurr
#Baptiste.Auguié      
#Joshua.Wiley
#James.Howison        
#wcyee
#the.aaron          
#Nick.S

df$Jean.Olivier.Irisson
median(df$Jean.Olivier.Irisson)
mean(df$Jean.Olivier.Irisson)

df$Ben
median(df$Ben)
mean(df$Ben)

df$Dennis.Murphy
median(df$Dennis.Murphy)
mean(df$Dennis.Murphy)

df$Roman.Luštrik
median(df$Roman.Luštrik)
mean(df$Roman.Luštrik)

df$Brandon.Hurr
median(df$Brandon.Hurr)
mean(df$Brandon.Hurr)

df$Baptiste.Auguié
median(df$Baptiste.Augui?)
mean(df$Baptiste.Augui?)

df$Joshua.Wiley
median(df$Joshua.Wiley)
mean(df$Joshua.Wiley)

df$James.Howison
median(df$James.Howison)
mean(df$James.Howison)

df$wcyee
median(df$wcyee)
mean(df$wcyee)

df$the.aaron
median(df$the.aaron)
mean(df$the.aaron)

df$Nick.S
median(df$Nick.S)
mean(df$Nick.S)

#############Bar plots####################
library('ggplot2')

df$fitems<-cut(df$Jean.Olivier.Irisson,breaks=c(0,1,2,3,4,5,6,7,8,9,10),labels=c("1","2","3","4","5","6","7","8","9","10"))
df$fitems<-cut(df$Ben,breaks=c(0,1,2,3,4,5,6,7,8,9,10),labels=c("1","2","3","4","5","6","7","8","9","10"))
df$fitems<-cut(df$Dennis.Murphy,breaks=c(0,1,2,3,4,5,6,7,8,9,10),labels=c("1","2","3","4","5","6","7","8","9","10"))
df$fitems<-cut(df$Roman.Luštrik,breaks=c(0,1,2,3,4,5,6,7,8,9,10),labels=c("1","2","3","4","5","6","7","8","9","10"))
df$fitems<-cut(df$Brandon.Hurr,breaks=c(0,1,2,3,4,5,6,7,8,9,10),labels=c("1","2","3","4","5","6","7","8","9","10"))
df$fitems<-cut(df$Baptiste.Auguié,breaks=c(0,1,2,3,4,5,6,7,8,9,10),labels=c("1","2","3","4","5","6","7","8","9","10"))
df$fitems<-cut(df$Joshua.Wiley,breaks=c(0,1,2,3,4,5,6,7,8,9,10),labels=c("1","2","3","4","5","6","7","8","9","10"))
df$fitems<-cut(df$James.Howison,breaks=c(0,1,2,3,4,5,6,7,8,9,10),labels=c("1","2","3","4","5","6","7","8","9","10"))
df$fitems<-cut(df$wcyee,breaks=c(0,1,2,3,4,5,6,7,8,9,10),labels=c("1","2","3","4","5","6","7","8","9","10"))
df$fitems<-cut(df$the.aaron,breaks=c(0,1,2,3,4,5,6,7,8,9,10),labels=c("1","2","3","4","5","6","7","8","9","10"))
df$fitems<-cut(df$Nick.S,breaks=c(0,1,2,3,4,5,6,7,8,9,10),labels=c("1","2","3","4","5","6","7","8","9","10"))
ggplot(data=df, aes(df$fitems)) +
  geom_bar(col="red") +
  labs(y="",x="") +
  scale_x_discrete(drop=FALSE) +
  scale_y_continuous(limits = c(0,5)) +
  theme(plot.title=element_text(face="bold"))
















###########ICC###########
#ICC1: Each target is rated by a different judge and the judges are selected at random. (This is a one-way ANOVA fixed effects model and is found by (MSB- MSW)/(MSB+ (nr-1)*MSW))
#ICC2: A random sample of k judges rate each target. The measure is one of absolute agreement in the ratings. Found as (MSB- MSE)/(MSB + (nr-1)*MSE + nr*(MSJ-MSE)/nc)
#ICC3: A fixed set of k judges rate each target. There is no generalization to a larger population of judges. (MSB - MSE)/(MSB+ (nr-1)*MSE) 

#install.packages("psych")
library('psych')
ICC(df$Jean.Olivier.Irisson)
ICC(df$Jean.Olivier.Irisson,missing=TRUE,alpha=.05)
ICC(df$Jean.Olivier.Irisson,missing=FALSE,alpha=.05)


#eerst alle votes van 1 deelnemer, NIET eerst alles votes van een user
sf <- matrix(c( 7, 3,   7,  2,  8,  6,  9,
                8, 9,   9,  4,  7,  7,  7,
               10, 9,  10,  7,  7,  9, 10,
                9, 8,   8,  9,  8,  5,  7,
               10, 9,  10,  8,  8,  8,  7,
               10, 9,  10, 10,  7, 10, 10,
               10, 7,  10,  8,  8,  5,  7,
                9, 5,   7,  4,  5,  6,  7,
                5, 3,   2,  4,  1,  3,  4,
                8, 3,   3,  1,  3,  5,  2,
                8, 3,   4,  1,  4,  5,  3),ncol=7,byrow=TRUE)
colnames(sf) <- paste("J",1:7,sep="")
rownames(sf) <- paste("S",1:11,sep="")
sf
ICC(sf)


#sf <- matrix(c(9,    2,   5,    8,
#               6,    1,   3,    2,
#               8,    4,   6,    8,
#               7,    1,   2,    6,
#               10,   5,   6,    9,
#               6,   2,   4,    7),ncol=4,byrow=TRUE)
#colnames(sf) <- paste("J",1:4,sep="")
#rownames(sf) <- paste("S",1:6,sep="")
#sf  #example from Shrout and Fleiss (1979)
#ICC(sf)