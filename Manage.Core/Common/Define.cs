using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core.Common
{
    internal class Define
    {
        internal const string _PASSWORDSPLIT = "$!&";   //密码分隔
        internal const string _USERCACHEKEY = "$!&2014";   //用户缓存密钥
        internal const string _TOPPARENTID = "0";   //树节点顶层父节点ID(父节点为该值，表示该节点为顶层)
        //公钥
        internal const string _PUBLICKEY = "<RSAKeyValue><Modulus>zRIC53+q4S17SjVCo0NAuPsR2rADPSh5wEhZBSW1OffLE8/g3j3dZeCoAa7Z2kGCyMSr+Ij+vQC+t563x6UbHUi9YyAeWjG+55muw7/dXFgxnixi/085K3dosG9zgZ/8MCrtpDZfRUVWIaz1Gcx+5O637FqQ4xSUGwlZu8XsY4M=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        //密钥
        internal const string _PRIVATEKEY = "<RSAKeyValue><Modulus>zRIC53+q4S17SjVCo0NAuPsR2rADPSh5wEhZBSW1OffLE8/g3j3dZeCoAa7Z2kGCyMSr+Ij+vQC+t563x6UbHUi9YyAeWjG+55muw7/dXFgxnixi/085K3dosG9zgZ/8MCrtpDZfRUVWIaz1Gcx+5O637FqQ4xSUGwlZu8XsY4M=</Modulus><Exponent>AQAB</Exponent><P>8FVjJm0374YmluUioKkrsI7HtYVytS8T91fSN9neP7NX8PPNArUUGd0fUpOgoXYWCV6jOVrcC2sE7BTm0POQjQ==</P><Q>2nApqiJe7+KbKjsylkAt+lHOh7p9TpHxQnlPKwWuQiUe9adp9lcVqgxRgUb6bjUjISVSW4/NWe87I+B4VY/oTw==</Q><DP>LxBppELLK2rX78DbcR7v4Vl0noWNmxGnFU7raeiOb2cNl7AGu7r+PrpgwekLEdNwKVGRIVA7uziv9BN7x/uKmQ==</DP><DQ>QNrAZBUkXtDHBPjigh24CPQ0/7Ns9OD74qKl0L41CMqAsKruGQeuFPjnUhzCyenY7kRoeWruq18ODg7da3n4nQ==</DQ><InverseQ>si444vVckdbcnMWX76HjzIxjarw2HRYQzEEiyY3HfBARwH+ya/Tc2MCqdi2Ms7G8kNTPPaByRAcvpor8ESkefg==</InverseQ><D>dXMuZKYzGVqyNTNB4j3fcNKjTCvo/vsSmDUXfyI0pGmuTr+Nm9u01OeRcb1SGnXQ0OoLUIIt29P0ZzgpK9f9KYbBDmZsbqPWoUBhex9HGc1oDnJBEz53Jve5uP1MNGQ5s59U0ioAUk2TEcqWKSY+2S67UEetR3Z7XVXezEmkjlk=</D></RSAKeyValue>";
        internal const string _SSOTICKETKEY = "X8PPNArU";   //SSO票据有效性验证码密钥
    }
}
