fsQ
===

*fsQ* gives you a few F# friendly *c.cs* KDB+ connector extensions 

(see KDB+ by Kx.com -- there's a free full featured 32 bit version there available for download).

###Focus:

* compatibility with the larger .net ecosystem through leveraging c.cs and staying away from F# features that are poorly supported elsewhere on the CLR.

* a few simple helpers like a blending Q Console and F# Interfactive (FSI).  For example, file.fsx:

    Q "til 10"                         

  returns the following when sent to FSI:

    q)til 10                           
    0 1 2 3 4 5 6 7 8 9      

###What is KDB+?

Probably KDB+ is going to be one of the fastest real-time and analytics database engines you'll ever come across.  Independent tests from https://stacresearch.com/ suggest that is at least the case for the production-style queries that they've designed.

KDB+ consists of:

* *q-sql*: a more expressive SQL that is succinct, compact, and adept at analytics

* the languages *q* and *k* (what *q-sql* is implemented in).  

* a mind-boggling amazing implementation in C by Arthur Whitney


A few things that KDB+ users enjoy:

* an agile, responsive programming experience.  

* KDB+ helps you to focus on solving business problems rather than prematurely optimising your solution to be skewed towards 
historical or real-time (as we are often forced to), because KDB+ has been shown to elegantly handles both cases.

* great for general purpose programming, but suits high performance computing equally well

* you don't have to throw away your shema


###What is this project about?

There are a few different parts:

* The connector itself is fairly solid.  The work has lead to a few minor improvements, mostly bug fixes to c.cs at code.kx.com.

* tuple, list, and array friendly

* the basics for doing language integrated exercises for learning and education purposes (more about that later).  

* client-side only formatter.  This is experimental.  It walks structures that are returned by q.exe.  Soon to be included:
  the .Q.s formatter, so that it will look exactly the same as the q.exe console.  
  The exercise was to walk returned structures.  So far this is partially achieved.

* more features, and possibly even a few unique ones, to come. 

###Why FSharp and KDB+?

Some feel that F# is a great companion to KDB+.  F# has a few things in common with KDB+:

* F# has a version of right-to-left (or left-of-right) in through its pipeline operators <| and |>.  While F# is more verbose than Q, F# is 
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
  will aim to provide this CLR support from F#.  For basic connector support for C# (and perhaps even F# depending on 
  what you are after), the c.cs connector to be found at code.kx.com is the most production ready.  The 
  c.cs code here is a little out of date (to be remedied), and it also contains a few minor extensions
  (such as support for +/- infinity, etc).

###Breaking news

The 32 bit version of KDB+ is now free to use in either a commercial or a non-commercial capacity.  That's huge.  The 32 bit version may sound humble, but *I think* a single threaded version of this outperformed many competing systems in recent history.  I'm just saying that this humble offering *still* packs a mean punch.  It's not to be sniffed at.  Not a bad place to start if you think that you might need to scale out in a painless way.


##Getting involved

Feel free to either lurk or get involved


###Why GitHub?

Most of the F# community lives on GitHub