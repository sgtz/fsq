fsq
===

So far, **fsq** provides a few F# friendly cs connector extensions to KDB+ by Kx Systems (kx.com).  In time, this may grow to have

###What is KDB+?

In the opinion of this writer at least, KDB+ is likely to be one of the fastest real-time and / or analytics databases you'll ever come across... and independent performance research from https://stacresearch.com/ suggests this is also the case for the benchmarks they've designed.  

KDB+ consists of:

* q-sql: a more expressive SQL that is succinct, compact, and adept at analytics

* the languages q and k (what q-sql is implemented in).  


###What is this project about?

There are a few different parts:

* The connector itself is fairly solid.  The work has lead to a few minor improvements, mostly bug fixes to c.cs at code.kx.com.

* client-side only formatter.  This is experimental.  It walks structures that are returned by q.exe.  Soon to be included:
  the .Q.s formatter, so that it will look exactly the same as the q.exe console.  
  The exercise was to walk returned structures.  So far this is partially achieved.

* tuple friendly

* the basics for doing language integrated exercises for learning and education purposes (more about that later).  

* more features, possibly even a few unique ones, to come. 

###Why FSharp and KDB+?

Some feel that F# is a great companion to KDB+.  F# has a few things in common with KDB+:

* F# has a version of right-to-left in the form of pipeline operators <| and |>.  While F# is more verbose than Q, F# is 
  more expressive and succinct than any other OO language on the CLR.  

* Lists and Sequences are at the heart of F# in the same way as these concepts are central to Q

* F# Interactive (FSI) is a little like Emacs with a REPL and IntelliSense.  APL (of which KDB+ shares heritage), has had 
  a REPL from the beginning i.e. 40+ years ago.  It's nice that the world is moving more in this direction more.
  
* F# has been a home for innovation on the CLR, such that other CLR based OO languages have 
  learned from F# by implementing several F# features, after being inspired by F#.
 
* F# does a very good job of integrating both functional and OO programming techniques

* you could do a lot of this from C# given the feature overlap, but it's not just about the features, 
  it is also about the way these features are expressed, and how this expression then 
  lends itself to problem solving and the Iversonian idea of, "notation as a tool of thought."

* In time this connector may grow to encompass support for other CLR languages.  This project
  will aim to provide this from F#.  For basic connector support for C# (and perhaps even F# depending on 
  what you are after), the c.cs connector to be found at code.kx.com is the most production ready.  The 
  c.cs code here is a little out of date here (to be remedied), and it also contains a few minor extensions
  (such as support for +/- infinity, etc).


##Getting involved

Feel free to either lurk or get involved


###Why GitHub?

Most of the F# community lives on GitHub