<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wx_open_auth.aspx.cs" Inherits="wx_open_auth.wx_open_auth" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br/>
            <h2>一键授权</h2>
            <a href="https://mp.weixin.qq.com/cgi-bin/componentloginpage?component_appid=<%=_ServerAppID%>&pre_auth_code=<%=_PreAuthCode%>&redirect_uri=<%=_RedirectUri%>">
                <img src="Content/image/icon_button_wx_auth.png" />
            </a>
        </div>
    </form>
</body>
</html>
