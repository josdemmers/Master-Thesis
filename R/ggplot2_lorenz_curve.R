install.packages("ineq")
library('ineq')

#Google Groups
#df <- read.csv('C:\\Users\\Uthar\\Dropbox\\TUE\\Master Thesis\\Master Thesis\\R\\GG_stats.csv',sep=';')
#df <- read.csv('C:\\Users\\s100026\\Dropbox\\TUE\\Master Thesis\\Master Thesis\\R\\GG_stats.csv',sep=';')
df <- read.csv('F:\\!Laptop\\Master Thesis\\data\\Google Groups\\Output\\GG_stats.csv',sep=';')
df <- read.csv('F:\\!Laptop\\Master Thesis\\data\\Stack Overflow\\SO_stats.csv', sep=';')

ineq(df$Answers,type='Gini')
#plot(Lc(df$Answers + df$Questions),
plot(Lc(df$Answers),
     xlab="User Percentile",
     ylab="Answer Percentage",
     main="Lorenz Curve - ggplot2 (GG)",
     col="blue"
     )
