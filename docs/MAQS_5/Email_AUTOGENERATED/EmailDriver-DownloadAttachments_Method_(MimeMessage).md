# EmailDriver.DownloadAttachments Method (MimeMessage)
 

Download all the attachments for the given message

**Namespace:**&nbsp;<a href="#/MAQS_5/Email_AUTOGENERATED/Magenic-Maqs-BaseEmailTest_Namespace">Magenic.Maqs.BaseEmailTest</a><br />**Assembly:**&nbsp;Magenic.Maqs.BaseEmailTest (in Magenic.Maqs.BaseEmailTest.dll) Version: 5.3.0

## Syntax

**C#**<br />
``` C#
public virtual List<string> DownloadAttachments(
	MimeMessage message
)
```


#### Parameters
&nbsp;<dl><dt>message</dt><dd>Type: MimeMessage<br />The email</dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/6sh2ey19" target="_blank">List</a>(<a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">String</a>)<br />List of file paths for the downloaded files

## Examples

**C#**<br />
``` C#
[TestMethod]
[TestCategory(TestCategories.Email)]
public void DownloadAttachmentsToConfigLocation()
{
    MimeMessage singleMessage = this.EmailDriver.GetMessage("Test/SubTest", "4");
    List<string> attchments = this.EmailDriver.DownloadAttachments(singleMessage);

    try
    {
        Assert.AreEqual(3, attchments.Count, "Expected 3 attachments");

        foreach (string file in attchments)
        {
            string downloadFileHash = this.GetFileHash(file);
            string testFileHash = this.GetFileHash(Path.Combine(EmailConfig.GetAttachmentDownloadDirectory(), Path.GetFileName(file)));

            Assert.AreEqual(testFileHash, downloadFileHash, Path.GetFileName(file) + " test file and download file do not match");
        }
    }
    finally
    {
        foreach (string file in attchments)
        {
            File.Delete(Path.Combine(EmailConfig.GetAttachmentDownloadDirectory(), Path.GetFileName(file)));
        }
    }
}
```


## See Also


#### Reference
<a href="#/MAQS_5/Email_AUTOGENERATED/EmailDriver_Class">EmailDriver Class</a><br /><a href="#/MAQS_5/Email_AUTOGENERATED/EmailDriver-DownloadAttachments_Method">DownloadAttachments Overload</a><br /><a href="#/MAQS_5/Email_AUTOGENERATED/Magenic-Maqs-BaseEmailTest_Namespace">Magenic.Maqs.BaseEmailTest Namespace</a><br />