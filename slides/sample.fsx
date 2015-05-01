﻿(**
- title : FsReveal 
- description : Introduction to FsReveal
- author : Karlkim Suwanmongkol
- theme : Sky
- transition : default

***

### What is FsReveal?

- Generates [reveal.js](http://lab.hakim.se/reveal-js/#/) presentation from [markdown](http://daringfireball.net/projects/markdown/)
- Utilizes [FSharp.Formatting](https://github.com/tpetricek/FSharp.Formatting) for markdown parsing

***

### Reveal.js

- A framework for easily creating beautiful presentations using HTML.  
  
> **Atwood's Law**: any application that can be written in JavaScript, will eventually be written in JavaScript.

***

### FSharp.Formatting

- F# tools for generating documentation (Markdown processor and F# code formatter).
- It parses markdown and F# script file and generates HTML or PDF.
- Code syntax highlighting support.
- It also evaluates your F# code and produce tooltips.

***

### Syntax Highlighting

#### F# (with tooltips)

*)
module Helpers =
    type System.String with
        member x.ContainsI(s:string) = if x = null then false else x.IndexOf(s,System.StringComparison.InvariantCultureIgnoreCase) >= 0

