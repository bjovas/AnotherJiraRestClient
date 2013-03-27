<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AspnetSampleApp.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>Another Jira Rest Client sample Asp.Net application</h1>
        <p>Click the buttons to trigger some sample exceptions, and log them from global.asax</p>  
    </div>
    <div>
        <h2>ArgumentException - Something fails directly on the website</h2>
        <p>
            <asp:Button ID="ArgumentExceptionButton" runat="server" Text="ArgumentException" OnClick="ArgumentExceptionButton_Click" />
        </p>
    </div>
    <div>
        <h2>OutOfMemoryException - Something fails in class library</h2>
        <p>
            <asp:Button ID="OutOfMemoryExceptionButton" runat="server" Text="OutOfMemoryException" OnClick="OutOfMemoryExceptionButton_Click" />
        </p>
    </div>
        <div>
        <h2>Database Exception - Fail when trying to access SQL server</h2>
        <p>
            <asp:Button ID="DatabaseExceptionButton" runat="server" Text="DatabaseException" OnClick="DatabaseExceptionButton_Click" />
        </p>
    </div>
    </form>
</body>
</html>