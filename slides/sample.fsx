(**
- title : F# intro via FsReveal
- description : Introduction to F# with help from FsReveal
- author : Brandon D'Imperio
- theme : Sky
- transition : default

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
let a = 5 // var a = 5;
let c = 1 + a // var c = 1 + a;
// public int Factorial(int x) => Enumerable.Range(1, x)
//  .Aggregate((x1,x2) => x1 * x2);
let factorial x = [1..x] |> List.reduce (*)
// var d = Factorial(a);
let d = factorial a
(**
`c` and `d` are evaluated for you
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
  let z = x = 6
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
// string F2(int lower, int x, int upper) => x < lower ? "less than" : x > upper ? "greater than" : "between";
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
  // var z = (int)y;
  // downcast when the type isn't inferrable (? mark notes the possibility of failure)
  let z:int = y :?> int
  // var a = (obj)x;
  let a:obj = upcast x // when the type is inferrable
  // var b = (int)y;
  // when the type is inferrable (still could fail)
  let b:int = downcast y
(**
---

#### Casting 2
*)

module Casting2 =
  // var x = (object)2;
  let x:obj = upcast 2
  // can't conditionally define a variable nor have it be the exact type
  // var a2 = x as int?;
  // if(a2 != null) Console.WriteLine("int value:" + a2);
  // else Console.WriteLine(x.ToString());
  match x with
  | :? int as a -> printfn "int value: %i" a
  | foo when isNull foo -> printfn "non-int null value is %A" foo // calls ToString if it is not null, a tuple, record or union
  | x -> printfn "nonNull value is %O" x // calls toString if it is not null

(**
***
### Syntax 2
#### fields vs methods
*)
module FExamples =
  let x = 1; // static field
  // int Y() => 1;
  let y () = 1 // method
  // void Z() {}
  let z () = ()
  // void F(int x) {}
  let f x = (); // method
  // void F2(int x) => f(x);
  let f2 = f // method

(**

---
#### Objects
*)
type Foo() = class end
// public class HelloClass {
type HelloClass() =
  //int x = "hello";
  let x = "hello" // field
  // public Foo() { return x;}
  member this.Foo() = x
  member this.Z = x
  static member Bar() = ()
(**
---
#### Interfaces
*)
type IAmAnInterface =
  abstract member Bark : unit -> string
  abstract member Foo : string with get
  abstract member Bar : string with get,set

type ClassImplements () =
  let x = "hello"
  // you can use whatever name you like for `this`
  member __.Bark() = x
  // is a property! not a field
  member __.Foo = x
  member __.Foo2 with get() = x
  member val Bar = x with get,set

  interface IAmAnInterface with
    member __.Foo = x
    member x.Bark () = x.Bark()
    member x.Bar with get() = x.Bar and set v = x.Bar <- v

(**
---
#### Records
*)

type Employee = {Name:string; EmployeeId:Guid; Salary:decimal}
let e = {Name="Brandon D'Imperio"; EmployeeId=Guid.NewGuid(); Salary = 15.5m}
let e2 = {e with Name="John Doe"; EmployeeId = Guid.NewGuid()}

(**
***
### Collections
#### Sequences

*)
module SequenceExamples =
  // new is not required in F#
  let a = System.Collections.Generic.List<int>()
  let literalArray = [| |]
  let arrayOf1To10 = [| 1..10 |]
  let literalList = [] // F# version is immutable
  let listOf1To10 = [ 1..10]

(**
---
#### C#

    [lang=cs]
    var items = new []{123,456,10,999,9};
    var q= from i in items
      join j in items on i equals j
      select new {i,j};



    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello, world!");
        }
    }
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
