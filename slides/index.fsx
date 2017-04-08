(**
- title : F# intro via FsReveal
- description : Introduction to F# with help from FsReveal
- author : Brandon D'Imperio
- theme : sky
- transition : default

***

### FsReveal notes #

- [http://fsprojects.github.io/FsReveal/getting-started.html](http://fsprojects.github.io/FsReveal/getting-started.html)
- `Esc` to see overview
- `f` to view in fullscreen
- `s` to see speaker notes

***
***

### INTRO TO F#

#### (For C#ers)

- by Brandon D'Imperio
- [imaginarydevelopment.blogspot.com](http://imaginarydevelopment.blogspot.com)
- [@MaslowJax](http://www.twitter.com/MaslowJax)

***

### Syntax

#### F# (with tooltips)

*)
open System
module Syntax = 
  let a = 5 // var a = 5;
  let c = 1 + a // var c = 1 + a;
  // public int Twice(int x) => x * 2;
  let twice x = 2 * x
  let d = twice a
(**
fsreveal magic `c` and `d` are evaluated for you
*)
(*** include-value: c ***)
(*** include-value: d ***)
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
  let y:obj = x :> obj
  // downcast when the type isn't inferrable (? mark notes the possibility of failure)
  // var z = (int)y;
  let z:int = y :?> int
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
  match x with
  | :? int as a -> printfn "int value: %i" a
  // calls ToString if it is not null, a tuple, record or union
  | null -> printfn "non-int null value is %A" x // would print <null> for null values
  // conditional matches
  | :? string as str when str.Length > 0 -> printfn "string value: %s" str
  // also prints <null> for null values
  | x -> printfn "nonNull value is %O" x // calls toString if it is not null
  // advanced/arguably misusable power: 
  // in F# we don't need to use a separate variable name for the casted variable

(**
***
### Syntax 2
#### fields vs methods
*)
module FExamples =
  // readonly static int x = 1;
  let x = 1 // static field
  // int Y() => 1;
  let y () = 1 // method
  // void Z() {}
  let z () = ()
  // void F(int x) {}
  let f (x:int) = () // method
  // void F2(int x) => f(x);
  let f2 x = f x // method

(**

---
#### Objects
    [lang=cs]
    public class Employee
    {
      int x="hello";
      public int Foo() => x;
      public int Z => x;
      public static void Bar() => {};
    }
    public class Foo{}
*)
type HelloClass() =
  let x = "hello" // field
  member this.Foo() = x
  member this.Z = x
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
  // you can use whatever name you like for `this`
  member __.Bark() = x
  // Foo is a property! not a field
  member __.Foo = x
  member __.Foo2 with get() = x
  member val Bar = x with get,set

  // f# downside no implicit interface implementation
  // interface members are always considered explicity implemented
  // members only show up/compile if you cast the type to the interface first
  interface IAmAnInterface with
    member __.Foo = x
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
// create a new employee with the same salary ... and any other properties that we don't set get copied.
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

*)
[<Measure>] type sqft
[<Measure>] type dollar
let sizes = [|1700<sqft>;2100<sqft>;1900<sqft>;1300<sqft>|]
let prices = [|53000<dollar>;44000<dollar>;59000<dollar>;82000<dollar>|]
(**

#### `prices.[0]/sizes.[0]`

*)
(*** include-value: prices.[0]/sizes.[0] ***)
(**

*)
