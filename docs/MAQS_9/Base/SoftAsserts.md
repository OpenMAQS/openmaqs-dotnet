# <img src="resources/maqslogo.ico" height="32" width="32"> Soft Assert

## Overview
Soft Asserts are a collection of assertions.  When a soft assert is called it will determine the results of a condition, along with a name for the soft assert, and a failure message, and save that to a collection of Soft Asserts.  It will also save the results of that Soft Assert to the log.
To call a Soft Assert using another assertion library in a test simply write 
```csharp
// Simple
SoftAssert.Assert(() => Assert.IsTrue(shouldBeTrue, "Expected true")); 

// More complex
SoftAssert.Assert(() => Assert.ThrowsException<StackOverflowException>(DoSomethingBadFunc));
```
If the success of a test is dependent on the results of the assertions, then the method 
```csharp
SoftAssert.FailTestIfAssertFailed();
``` 
will be called, and if any previous SoftAsserts have been determined to have failed, it will throw an exception with the failed assert names, and the failure messages associated with those names.  
The test will also write to the log with the end results of the Soft Assert collection.

## Uses
Soft Asserts are commonly used when collecting large amounts of data that needs to be evaluated without affecting the results of a test.  In unit testing, Asserts will throw an exception if their condition fails.  With Soft Asserts multiple assertions may be made, and stored, to be evaluated later.  They make aggregate that assertion data into one place to be evaluated.

## Soft Assert Conditionals

### Assert(Action)
Assert will consume any standard assert. If the consumed assertion fails it will store that assert as a failure. If the the consumed assertion does not fail it will store that result as a success.

#### Written as
```csharp
SoftAssert.Assert(() => Assert.IsTrue(shouldBeTrue, "Expected true")); 
```
#### Examples
```csharp
// Results in a true assertion
SoftAssert.Assert(() => Assert.IsTrue(true, "True assertion")); 

// Results in a false assertion
SoftAssert.Assert(() => Assert.IsFalse(true, "False assertion")); 

// Throws assertion
SoftAssert.Assert(() => Assert.ThrowsException<StackOverflowException>(DoSomethingBadFunc));
``` 
Assert will check the provided assertion function. If an assertion exception is thrown it will store that assert as a failure. If no assertion exception is thrown it will store that result as a success.

#### Pass in multiple asserts

```csharp
// Results in a false assertion
SoftAssert.Assert(() => 
{
    Assert.IsTrue(true, "True assertion");
    Assert.IsFalse(false, "True assertion"));
});

// Results in a false assertion, Only the first two asserts run
SoftAssert.Assert(() => 
{
    Assert.IsTrue(true, "True assertion");
    Assert.IsTrue(false, "False assertion"));
    Assert.IsTrue(false, "False assertion"));
});
```

Assert can also combine multiple asserts together. If a single assertion exception is thrown it will store that assert as a failure. On the first failure the method will no invoke any more lines of code, so be sure to use multiple Assert methods if certain items must be checked.  If no assertion exception is thrown it will store that result as a success.

## Expected Soft Asserts
Expected soft asserts allow you to include a list of soft asserts which must be run in order for a test to pass. This helps assure that all expected soft asserts are run.

### SoftAssertExpectedAsserts(Attribute)
The SoftAssertExpectedAsserts attribute allows you to add the expected asserts to your test method as an attribute.

#### Written as
```csharp
[SoftAssertExpectedAsserts("one", "two")]
```
#### Examples
```csharp
[TestMethod]
[SoftAssertExpectedAsserts("one", "two")]
public void TEST()
{
    this.SoftAssert.Assert(() => Assert.IsTrue(buttonOne.Enabled), "one");
    this.SoftAssert.Assert(() => Assert.IsTrue(buttonTwo.Enabled), "two");

    // Will fail is soft assert one and two have not been run
    this.SoftAssert.FailTestIfAssertFailed();
}
``` 

### AddExpectedAsserts(Action)
The AddExpectedAsserts function allows you to add the expected asserts within your tests code.

#### Written as
```csharp
this.SoftAssert.AddExpectedAsserts("one");
```
#### Examples
```csharp
[TestMethod]
public void TEST()
{
    this.SoftAssert.AddExpectedAsserts("one");
    this.SoftAssert.AddExpectedAsserts("two");

    this.SoftAssert.Assert(() => Assert.IsTrue(buttonOne.Enabled), "one");
    this.SoftAssert.Assert(() => Assert.IsTrue(buttonTwo.Enabled), "two");

    // Will fail is soft assert one and two have not been run
    this.SoftAssert.FailTestIfAssertFailed();
}
``` 

## Soft Assert Collection Handling
After assertions have been made, and the soft assert collection has been filled with the results of those assertions, comes options on how to handle the results of that collection.
### Fail the Test if Any Soft Assert Failed
If the results of a collection of Soft Asserts would fail a test due to the tests conditions, then the FailTestIfAssertFailed method would be called.  The method will throw an exception with any Soft Asserts that failed, and it will write to the log with the final results.

#### Example
```csharp
// Checks the Soft Assert collection, fails the test if a soft assert failed
SoftAssert.FailTestIfAssertFailed();
``` 

#### Example output
```
ERROR:	SoftAssert.AreEqual failed for .  Expected '2' but got '1'.  False assertion
Total number of Asserts: 6. Passed Asserts = 3 Failed Asserts = 3
``` 

### Send All Soft Assert Data to the Log
If the results of a test aren’t dependent on the results of the collection of SoftAsserts, the method LogFinalAssertData may be called.  The method will write to the log the results of the Soft Assert collection, giving a record of any failed or passed results.  Failed soft asserts will be written as warnings.

#### Example
```csharp
// Writes the final assert data to the log without affecting the results of the test
SoftAssert.LogFinalAssertData();
``` 
#### Example output
```
WARNING:	Soft Assert 'False assertion' failed. Expected Value = '2', Actual Value = '1'.
Total number of Asserts: 6. Passed Asserts = 3 Failed Asserts = 3
``` 

### Check If a Soft Assert Failed
If the results of a test aren’t dependent on the results of the collection of SoftAsserts but they may influence some future behavior of the test, DidSoftAssertFail may be called.  The method will returns a Boolean of either true, there were no failures, or false, there were failures.

#### Example
```csharp
// Will return true if no asserts failed, false if any asserts failed
bool softAssertResults = SoftAssert.DidSoftAssertsFail();
``` 

### Did the User Check for Failed Asserts
If any of the previous Soft Assert handler methods are called, it will set a property, that by default is faulse, to true.  DidUserCheck will return the value of that property.  At the end of a test the DidUserCheck method is called, if a SoftAssert conditional has been created since the last time the user checked the results of the Soft Assert collection, it will write to the log that the user has not checked the results.  It won’t affect the results of the test, but it will provide additional information to the log.

#### Example
```csharp
// Will return true if LogFinalData, FailTestIfAssertFailed, or DidSoftAssertFail was called
bool didUserCheck = SoftAssert.DidUserCheck();
``` 