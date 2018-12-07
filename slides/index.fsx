(**
- title : F# intro via FsReveal
- description : Introduction to F# with help from FsReveal
- author : Brandon D'Imperio
- theme : sky
- transition : default

***


### INTRO TO F#

#### (For C#ers)

- by Brandon D'Imperio
- [imaginarydevelopment.blogspot.com](http://imaginarydevelopment.blogspot.com)
- [@MaslowJax](http://www.twitter.com/MaslowJax)

---
### FsReveal notes #

- [http://fsprojects.github.io/FsReveal/getting-started.html](http://fsprojects.github.io/FsReveal/getting-started.html)
- `Esc` to see overview
- `f` to view in fullscreen
- `s` to see speaker notes

***

### Syntax

#### F# (with tooltips)
' module is like a static class, but no constructor is allowed

*)
open System
module Syntax =
  let a = 5 // var a = 5;
  let c = 1 + a // var c = 1 + a;
  // public int Twice(int x) => x * 2;
  // the () are optional
  let twice(x) = 2 * x
  // the () are optional
  let d = twice(a)
(**
fsreveal magic `c` and `d` are evaluated for you
*)
(*** include-value: Syntax.c ***)
(*** include-value: Syntax.d ***)
(**

---

#### Assignment

*)
module Assignment =
  // let is a binding not an assignment
  // = is a binding or comparison not an Assignment
  let x = 5 // not mutable
  let mutable y = 5
  // parens are just to help visualize it for new learners
  let z = (x = 6)
  y <- y + 1

(**
`Assignment.z` evaluates to
*)
(*** include-value: Assignment.z ***)
(**

---
### Flow Control
#### if
' consider touching on automatic generalization here
*)
// string F1(int x) => x < 5 ? "less than" : "not less than";
let f1 x = if x < 5 then "less than" else "not less than"
// string F2(int lower, int x, int upper) =>
//    x < lower ? "less than" : x > upper ? "greater than" : "between";
let f2 lower x upper =
  if x < lower then
    "less than"
  else if x > upper then // or elsif
    "greater than"
  else "between"

(**

---

#### Casting
*)
module Casting =
  let x = 5 // var x = 5;
  // var y = (obj) x;
  // upcast (always succeeds if it compiles)
  let y = x :> obj
  // downcast when the type isn't inferrable (? mark notes the possibility of failure)
  // var z = (int)y;
  let z = y :?> int
  // var a = (obj)x;
  let a:obj = upcast x // when the type is inferrable
  // var b = (int)y;
  // when the type is inferrable (still could fail)
  let b:int = downcast y
  // var x2 = (object)2;
  let x2:obj = upcast 2
(**
---

#### Casting 2
*)

module Casting2 =
  // box is a shorthand for upcast to object
  let x:obj = box 2
  // in C#5(maybe even 6) you can't conditionally define a variable
  // var a2 = x as int?;
  // if(a2 != null) Console.WriteLine("int value:" + a2);
  // else Console.WriteLine(x.ToString());
  let result =
    match x with
    | :? int as a -> sprintf "int value: %i" a
    // calls ToString if it is not null, a tuple, record or union
    | null -> sprintf "non-int null value is %A" x // would print <null> for null values
    // conditional matches
    | :? string as str when str.Length > 0 -> sprintf "string value: %s" str
    // also prints <null> for null values
    | x -> sprintf "nonNull value is %O" x // calls toString if it is not null
    // advanced/arguably misusable power:
    // in F# we don't need to use a separate variable name for the casted variable
(*** include-value: Casting2.result ***)

(**
***
### Syntax 2
#### fields vs methods
' c# static readonly vs const https://stackoverflow.com/questions/755685/static-readonly-vs-const
*)
module FExamples =
  // readonly static int x = 1;
  let x = 1
  let z () = () // void Z() {}
  let y () = 1 // int Y() => 1;
  // void F(int x) {}
  let f (x:int) = () // method
  // void F2(int x) => F(x);
  let f2 x = f x // method

(**

---
#### Objects
    [lang=cs]
    public class Employee
    {
      readonly string x="hello";
      public int Foo() => x;
      public int Z => x;
      public string Y {get;set;}
      public static void Bar() => {};
    }
    public class Foo{}
*)
type HelloClass() =
  let x = "hello" // field
  member this.Foo() = x
  member this.Z = x
  member val Y = null with get,set
  static member Bar() = ()
// empty class
type Foo() = class end
(**
---
#### Interfaces
*)
type IAmAnInterface =
  abstract member Bark : unit -> string
  abstract member Foo : string with get
  abstract member Bar : string with get,set
(**
---
*)
type ClassImplements () =
  let x = "hello"
  member this.Bark() = x
  // Foo is a property! not a field
  member this.Foo = x
  // you can use whatever name you like for `this`
  member __.Foo2 with get() = x
  // this creates a backing field of its own, using x's value as the initial value
  member val Bar = x with get,set

  // f# downside no implicit interface implementation
  // interface members are always considered explicity implemented
  // members only show up/compile if you cast the type to the interface first
  interface IAmAnInterface with
    member __.Foo = x
    // this isn't recursive
    member this.Bark () = this.Bark()
    member this.Bar
      with get() = this.Bar
      and set v = this.Bar <- v

(**
---
#### C#
    [lang=cs]
    public class Employee
    {
      public string Name { get; } // C# 6 syntax
      public Guid EmployeeId { get; }
      public decimal Salary { get;}

      public Employee(string name, Guid employeeId, decimal salary)
        Name=name;
        EmployeeId=employeeId;
        Salary=salary;
      }
    }


#### Records
*)

type Employee = {Name:string; EmployeeId:Guid; Salary:decimal}
let e = {Name="Brandon D'Imperio"; EmployeeId=Guid.NewGuid(); Salary = 15.5m}
// how much typing would you need to do in C# to do this?
// create a new employee with the same salary
// and any other properties that we don't set get copied.
let e2 = {e with Name="John Doe"; EmployeeId = Guid.NewGuid()}

(**
***
### Collections
#### Sequences

*)
module SequenceExamples =
  // new is not required in F#
  // you will may get a warning though if you don't use new on IDisposables
  let a = System.Collections.Generic.List<int>()
  // same as the line above (using build-in f# aliases)
  let b = ResizeArray<int>()
  let literalArray = [| |] // also Array.empty
  // var arrayOf1To10 = Enumerable.Rangea(1, 10).ToArray();
  let arrayOf1To10 = [| 1..10 |]
  // F# list type
  let literalList = [] // F# version is immutable
  let listOf1To10 = [ 1..10 ]

(**
---
#### C#

    [lang=cs]
    var items = new []{123,456,10,999,9};
    var q= from i in items
      join j in items on i equals j
      select new {i,j};
---
#### Linq
' touch on lambda syntax here
*)

module NotLinq =
  // var items new [] {123, 456,10,999,9};
  let items = [123;456;10;999;9]
  // var doubled = items.Select(x => x * 2);
  let doubled = items |> Seq.map (fun x -> x * 2)
  // public IEnumerable<int> Evens(IEnumerable<int> items) => items.Where(x => x % 2 == 0);
  let evens items = items |> Seq.filter (fun x -> x % 2 = 0)


(**
---
#### Query Form
*)
module Querying =
  let items = [123;456;10;999;9]
  let q =
    query {
        for i in items do
        join j in items on
            (i = j)
        select (i,j)
    }
(**
***
#### Units of Measure
' consider discussing type aliases

*)
[<Measure>] type inch
[<Measure>] type foot
[<Measure>] type sqft = foot*foot
[<Measure>] type dollar
let sizes = [|1700<sqft>;2100<sqft>;1900<sqft>;1300<sqft>|]
let prices = [|53000<dollar>;44000<dollar>;59000<dollar>;82000<dollar>|]
let inchesPerFoot = 12<inch/foot>
// can't seem to get this working here, nor in linqpad
// let numLiteral = 12_000<dollar>
(**

#### `prices.[0]/sizes.[0]`

*)
(*** include-value: prices.[0]/sizes.[0] ***)
(**
***
#### Not Covered, barely scratched surface, or not covered well
### Good
- Pattern matching (exhaustive matching and compiler warnings)
- Option types
- Units of measure conversions
- Discriminated Unions (also recursive DUs)
- Tuples (easier and more useful)
- Object expressions (we don't need a class or record to implement an interface)
- Computation expressions
- keywords - `rec`, `function`, `async`, `lazy`, `_`
- Tupled vs curried method forms

---
- Statically resolved type parameters (structural typing)
- Unwanted features (don't have, and don't want) - implicit casts
- Underscores in numeric literals as of 4.1
- """ triple quoted strings """
- Extension methods, properties, events, static methods
- shadowing (`let x = 1; let x = x + 1;`)
- signature files
- There are all of 3 features in C# since 2.0 that F# didn't have before C# did (citation needed, if true)

---
### Bad-ish
- Generic Measures can be difficult to work with
- Missing features (`nameof` operator, covariance/contravariance)
- Interop(ugliness of delegate interop)
- Nested classes
- No implicit interface implementations
- No good tooling for UI work (mvc, wpf, winforms, webforms, etc.)
- try reflection walking a namespace to get modules
- works in unity, but can't be main assembly
- works in mvc, but can't write razor pages in it
- can't T4

***
#### Resources
- Try F# in the browser - http://www.tryfsharp.org/ / https://dotnetfiddle.net/ - https://dotnetpad.net/
- Lots of good learning on F# - http://fsharpforfunandprofit.com/

*)
