# SoftAssert.DidUserCheck Method 
 

Gets a value indicating whether the boolean if the user checks for failures at the end of the test.

**Namespace:**&nbsp;<a href="#/MAQS_5/BaseTest_AUTOGENERATED/Magenic-Maqs-BaseTest_Namespace">Magenic.Maqs.BaseTest</a><br />**Assembly:**&nbsp;Magenic.Maqs.BaseTest (in Magenic.Maqs.BaseTest.dll) Version: 5.3.0

## Syntax

**C#**<br />
``` C#
public virtual bool DidUserCheck()
```


#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/a28wyd50" target="_blank">Boolean</a><br />If the user checked for failures. If the number of asserts is 0, it returns true.

## Examples

**C#**<br />
``` C#
/// <summary>
/// Test to verify that the did user check will be set back to false if they check for failures
/// </summary>
[TestMethod]
[TestCategory(TestCategories.Utilities)]
public void SoftAssertVerifyCheckForFailures()
{
    SoftAssert softAssert = new SoftAssert(new FileLogger(LoggingConfig.GetLogDirectory(), "UnitTests.SoftAssertUnitTests.SoftAssertVerifyCheckForFailures"));
    softAssert.AreEqual("Yes", "Yes", "Utilities Soft Assert", "Message is not equal");

    softAssert.FailTestIfAssertFailed();
    Assert.IsTrue(softAssert.DidUserCheck());

    softAssert.AreEqual("Yes", "Yes", "Utilities Soft Assert", "Message is not equal");
    Assert.IsFalse(softAssert.DidUserCheck());
}
```


## See Also


#### Reference
<a href="#/MAQS_5/BaseTest_AUTOGENERATED/SoftAssert_Class">SoftAssert Class</a><br /><a href="#/MAQS_5/BaseTest_AUTOGENERATED/Magenic-Maqs-BaseTest_Namespace">Magenic.Maqs.BaseTest Namespace</a><br />