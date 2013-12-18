<%@ Control Language="C#" AutoEventWireup="true" Inherits="M_UserControls_ShareSocial" Codebehind="ShareSocial.ascx.cs" %>
<table border="0">
    <tr>
        <td style="width: 150px;">
            <script src="http://connect.facebook.net/tr_TR/all.js#xfbml=1"> </script>
            <fb:like layout="button_count" show_faces="false" width="90">
            </fb:like>
        </td>
        <td style="width: 150px;">
            <a href="http://twitter.com/share" class="twitter-share-button" data-count="horizontal">
                Tweet</a><script type="text/javascript" src="http://platform.twitter.com/widgets.js"> </script>
        </td>
    </tr>
</table>
<table border="0">
    <tr>
        <td style="width: 150px;">
            <iframe src="//www.facebook.com/plugins/like.php?href=http%3A%2F%2Fwww.facebook.com%2FYucelVakfi&amp;send=false&amp;layout=button_count&amp;width=150&amp;show_faces=false&amp;font&amp;colorscheme=light&amp;action=like&amp;height=21&amp;appId=250538258380371" scrolling="no" frameborder="0" style="border: none; overflow: hidden; width: 120px; height: 21px;" allowtransparency="true"></iframe>

        </td>
        <td style="width: 150px;">
            <iframe allowtransparency="true" frameborder="0" scrolling="no" src="//platform.twitter.com/widgets/follow_button.html?screen_name=yucelvakfi&amp;show_count=true&amp;show_screen_name=false&amp;lang=tr" style="width: 150px; height: 21px;"></iframe>
        </td>
    </tr>
</table>