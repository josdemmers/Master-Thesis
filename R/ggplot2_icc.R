###########ICC###########
#ICC1: Each target is rated by a different judge and the judges are selected at random. (This is a one-way ANOVA fixed effects model and is found by (MSB- MSW)/(MSB+ (nr-1)*MSW))
#ICC2: A random sample of k judges rate each target. The measure is one of absolute agreement in the ratings. Found as (MSB- MSE)/(MSB + (nr-1)*MSE + nr*(MSJ-MSE)/nc)
#ICC3: A fixed set of k judges rate each target. There is no generalization to a larger population of judges. (MSB - MSE)/(MSB+ (nr-1)*MSE) 

#install.packages("psych")
library('psych')
#ICC(df$Jean.Olivier.Irisson)
#ICC(df$Jean.Olivier.Irisson,missing=TRUE,alpha=.05)
#ICC(df$Jean.Olivier.Irisson,missing=FALSE,alpha=.05)


#Each column represents the votes given by a participant.
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