using System;
using System.Text;
using OffLogs.Business.Extensions;
using OffLogs.Business.Helpers;
using Xunit;

namespace OffLogs.Api.Tests.Unit.Helpers.SecurityUtilsTests
{
    public class PasswordHashGenerationTest
    {
        [Theory]
        [InlineData("test", "testSalt", "xm3fHlaofGwb0YGXdLPWis1saDXFx99s8GGTag4jnhv30/zhxVx9CiE0o0k5fFmL061byVvg8/dIq+RqYFT8u18nbsEIT9wG8BbInlx1+t3lhTWOHGLwnfjy2LqGzkK6meZQaDrlouevOX1LblKbYVibIH1a0x2O9qbZbdEXwuns8GKDiGY4BnX0F/pAocdYDMAasPyii45g6thjEglL5YYxaAd+Y2D0amXgn4FbkejGVwCSLD9iXzceNpuv/GAUrGlKZPqjPj54RP8doZWRKEkxD3snzy5mRlywwUWwvJufiuirR9n+JHytbAGtdCW2VzP+Zu0+claD8vemftYegqs165ftkiVVgz7qwPL2o55rtcfSyS1/3sG6OiEXg/Fd60/kb46UcFnMMx9vAxFcZP/0mwF9K3+CDd/jC1irfzi4rIkUY8xoOVpGFV/HXeTmQscDDdEtkW39zKXNLrmU+4JUoCX6qdv4mpfEXlgboZhV142sjLndfTzwC3fIVuZtYAXIkDYoIlNnJGFeEWBHoTOQDgIyhzxHaXW0QGoFi8mNjP2IFIJDtLsFtPbhsjsSCqnpmtJSGEgDHOLA7Naid9VU/YFsQbLQrpGyUUw/YgKveIUOEusw534WuUJeMHwA9yLcF90UXjPkk478xsVcyyDu9hVZDRSpeMY5oyIBRDsRhI1EX+jJCfnJiw4vUzwo6WXCzyNfFmgffvbjlg+Mx72O+t+Q5PwauxU3vVvR2I5Wv5M49HTFy+BJUlL+Vo51XkIWvT1LVyL3n0yZob4h2Z36HeYZ6aSZ0ee6JNBsBfNW5EQt0UMTJOsmhwzq3M/Eh08utKTTrYiz1v/mxpqSPMOvHgnKtJ2WtJi0xF6YycU50F37pr2IcC5+qb+EFLeMuVB6hamtK0fpE5GVzMY9aF8hInipfi9IaIgQZiKxnPJfwTQX8K34g2mnWEW5FTf8ZCl1QvQCbGUZ8GGfBtapYGL27sEgphVNuD3XEcQFDHXxTkL5cDrIqpCG6knfrIvNy+F9Cs8O+EQEo8Tm4FDgaGXPYERqqXqeIRX6DdGgA4zm1u2h8AZqJ5EFjjv6qYJvMo4iCGsfUVXx8E+xPA4sVzdsMvGLhHFDZMhdugp5MfV/xodIn5pD25IsFsxjNccikzwjoZCt7U59phSsPqCHqgmR+Z07zYJ47+70PNbvtERxffVmWtPEJJyU2Y+gavXfhK06JPOa6EZW26H+fnMXr/QHfxUUHpwCovpDBHiDFZ6SFuRbbCYoAaK/apuDX7p5/DS4HyZsKamzCbqlIwieSqI8A4LSkc2b/lwJHGzV5VnXZ2iaaAGxmGqiPN5rwIKbPVjSFEQbfpvk4YRqwy0/")]
        [InlineData("D0YqmyTVocuiqkvs", "Z5HWGD66KWxKeIZr", "Bb+pakg1N2qItTLcoALELFsggKTWcF1SbnvxzKeP6Hd0PZD6a6xmiQWM46ukaeTL7xRHm1kB61dqj8vwKSD+h3w8aopcGroWaj2170iVUR/JpRTeT7A9cFpF+u7X4Uw2iFlmU2rxyKT2XYI644wUU/O7QLHFlKflcAVkXLCVltrKfeoyXffChYGjU92z3CV+UDyVTe5Obgi+xbgYSQ6Alo6AHfAhkFgnXJiXJ30IcNXo6yQuG7XgtOQ3Ik3eL1OvELbq50Yxb9o4RBtRbjB0XQrb65BT4QdHU2MqHr5VzC0kACccUbXQCJNEK44xg26EK6PSj04vPMZubN9DCch0nFlCoCeIDoPwaVf1a8uIuHybfA6+tJZaIE7cj/rMriUTZAHPAmheGip1gIm22maigXyO/+mqg6Xt/TEgsJegKyyVrlNgbQU1UrZhtu7OQXdFvt52o0dsHfAPJXRDFGloP9fNBqjLV3tIrwUSbs+DBlqzAFTIMsdXr/lj+KoXQqhg12mEmjV3YBBD+zfdN9UeFcToA0qTJV7oToszk/zeFxhuUBP/ZU0bMDl9jPkLwXbuK3mniEqveWtZtYSLB6CawGCXSquPzBSiFkh5pNHEpctQtvGXMBgU7xTERdtMNe8dcWivwJt1yqKBBkfhOzo76MqvoJLM0x7scW148bYmqGPu+y4qJwbLfwDT1JrUQxCMaBmPwr0IKCbvEHZcCS7orkDClr+hj8CzvDN5WA3/vnJrzE82DdD8xv2n4qz4YYXIBn01nSOMbIrdkdqlaGwMMWnLmKa4AoyFgMFbVvk+ENz/ww0MhhSPodbVN0x4In9QmcwCS/hD4wn1LS0bnGbD/ctezg8q27mBWApeKhAFYjByfP5JumEl6h0fRB5E52UYQMmadEUr2gEi4F4nqt5dfxfMrK8lGF2ocDRrioTDKNKGiwWo3Ib8hNogLMHWV+t4bGZoZZ7VjKboX0OET4KZsZos+pYcjkz1wA/1pn5R6jTni5V6Kx4V3chddCm6JSbbo4wWzYsRJSr/D46b4D+C/Fd2opJ0gFbR3tTqh4a5Wo8eci5Gr3a7xcpZyzSCyHSsedwvAawa1fzZNaGlvim9Xvqw6jyPuUCvQWjWsXe0ROKWfF7RNrBnp9WSbSJbKwU+FZMUpwV6Su/YhZLUJO2W9hLMINxIulx8XKug88JY7+8WB7OtfH+giWbVzHyxT1kXv0Ts4lHH/zO4VYvS2R+0sXxxDocWmejGtW/DEFNAq6veHf+rpRYUofsu75lIeEowJH3/FFyRyHK/iwvdFloTyT2zim96uG3QLhHx/Z8PiZsBlo2XCNyZe0K/P6zJ6OVznfQRhTRnSumOxyT6w3aA")]
        [InlineData("HJ14rlm1uYPsrPSu", "4QazLMVbGdrGGMCt", "404QKaS0WE8+fMbamhJhPTJbSYYDMFNUrb5KaWEPduJlWmfq21h5OrjJ1xfvDMjAnv5Mb7Lf7KefsiRVx+qBmXFVttY+wQk3keVG/5LZk3kSbW7LwKpMh5Xww7SuI4VerX95q+aqEJiDpVT5JdK3nNLfE3LS+zvEaiX4YaGfZoDUiLUBmKumTzzyzJ9yhmpzyC1d4CGqkBNgistmoaEziAjVnQqI4BazWDcIIDC31SAea1yYLV5aRQLE4VNQRqbgwBd9oXU+KVJ97VS+60wEPdzQ3ovYU9yNjtR7t9qETgcDb0NrYeS8veSA1G3CM2RK0bVFtvTR/vH2FagbFVc9GffDF4mOP+UGvqcZ78wbCTVRy7SHDEKts6ui026xJkNBANgZn7PFafaBSG0gaSDb86xiIZ9cR/Xvn93fPRnc4IUbpxgPIe2NLXFfmHBujqIkxguLO0xyOYoxUBthe8TmePrbFjR5MW1eqoxEfdl2gfwIitGDnhLL4V/IRTRZ941FAWQXqAN3zFpr9H6Ysd/L7OUq0lvU53/gylN0vz1aaDn8AsRfitTCkStOc/t8e2P69738Qg5uEs73fuExBklo5cbCl5XtYgLj+y+QPV2F0x7duBtFYL5Hw+0jrX6DGPqS9K8XCWR4WtjJrU0oiQoOj1VVBxvAOknTHQE84jApbJ6VpGvpz4HtMHF79wZvM7ve98XR0ABNGtRw0rzAwh5pBCa0TfE04z3+PQbxR3/sPHIva80ItYNklIo25KRAU/O5LbJ9aZdxfXmEfJjp8n+vmoCA1lSoExrxWB5nIL4+acoLWlKOHEJRtJhftDs/QWnkL7sILLxX5AEkHQyUmVAZFL8/8tP5ejHKK0pjXAqJRwllJY411d40xp0TV/LbdTaVW+hrrrSvVCXp4qYSctet2m99rSTHCVaIdU/0gaPQF6cIifWDGAJutPAvCD1Nx+AEILPR89mWC4T3X5Q7O4FHcI7ylKk8G0kyjaYODEnoldzGYmTMcSoh462nyvmc11oC4mkEqSbO9uUvB1mEcC26c6arR9/oCD0Lz47qvNBgPwJacGq4JBkFiL/iPY+FF1agtsJT2oyfdAD9C2f0qCH4qGqBRJ9ArSnwMClBz00hI6TKTwZkb04zJP9ygBB7eVWET3I1mwnc1fGrReAuEmkhDd6+aBZZT/sE3+t+rNHmcWJsnewv1q+A/IOSXtQHkmdwiMuEzMo/6++oeVJxeAXBGQ+ZmP5tdZ11E/S0pbwy2f1ZriS4WYOc3/0lhpnoK9WYqjO+QAvFlErFiLvlHFOodvw+X8oJRaCnj1j/hJ/kQmgsxiyv/xDNBADFXXwL5AHdmEf0AiWPBflrygnlARnS")]
        public void ShouldGeneratePasswordHash(string password, string salt, string expectedHashBase64)
        {
            var hashBytes = SecurityUtil.GeneratePasswordHash(
                password,
                Encoding.UTF8.GetBytes(salt)
            );
            var bytesBase64 = Convert.ToBase64String(hashBytes);
            Assert.Equal(expectedHashBase64, bytesBase64);
        }
    }
}