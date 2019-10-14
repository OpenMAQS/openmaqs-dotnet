# FluentMobileElement.TestObject Property 
 

Gets the test object for the element

**Namespace:**&nbsp;<a href="#/MAQS_4/Appium_AUTOGENERATED/Magenic-MaqsFramework-BaseAppiumTest_Namespace">Magenic.MaqsFramework.BaseAppiumTest</a><br />**Assembly:**&nbsp;Magenic.MaqsFramework.BaseAppiumTest (in Magenic.MaqsFramework.BaseAppiumTest.dll) Version: 4.0.4.0 (4.0.4)

## Syntax

**C#**<br />
``` C#
public AppiumTestObject TestObject { get; }
```


#### Property Value
Type: <a href="#/MAQS_4/Appium_AUTOGENERATED/AppiumTestObject_Class">AppiumTestObject</a>

## Examples

**C#**<br />
``` C#
[TestMethod]
[TestCategory(TestCategories.Selenium)]
public void FluentElementGetTestObject()
{
    FluentElement testFluentElement = new FluentElement(this.TestObject, By.CssSelector("#ItemsToAutomate"), "TEST");
    Assert.AreEqual(this.TestObject, testFluentElement.TestObject);
}
```


## See Also


#### Reference
<a href="#/MAQS_4/Appium_AUTOGENERATED/FluentMobileElement_Class">FluentMobileElement Class</a><br /><a href="#/MAQS_4/Appium_AUTOGENERATED/Magenic-MaqsFramework-BaseAppiumTest_Namespace">Magenic.MaqsFramework.BaseAppiumTest Namespace</a><br />