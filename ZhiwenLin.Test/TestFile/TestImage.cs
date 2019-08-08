using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace ZhiwenLin.Test.TestFile
{
    public class TestImage
    {
        [Fact]
        public void IsValidImage_Should_return_true_when_is_jpg()
        {
            // arrange
            var controller = new ProfileController();
            var privateObject = new PrivateObject(controller);
            bool result;

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "/testjpg.jpg");
            if (!File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    byte[] bytes = Convert.FromBase64String(GetJpg());
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                    fs.Close();
                }
            }

            // act
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var response = privateObject.Invoke("IsValidImage", fileStream);
                result = (bool)response;
            }

            // assert something

            File.Delete(filePath);
        }

        [Fact]
        public void IsValidImage_Should_return_false_when_is_html()
        {
            // arrange
            var controller = new ProfileController();
            var privateObject = new PrivateObject(controller);
            bool result;

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "/testhtml.html");
            if (!File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {

                    byte[] bytes = Encoding.Default.GetBytes(GetHtml());
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                    fs.Close();
                }
            }

            // act
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var response = privateObject.Invoke("IsValidImage", fileStream);
                result = (bool)response;
            }

            // assert
            File.Delete(filePath);
        }

        private string GetHtml()
        {
            return "<html>\r\n  <head>\r\n    <title>HTM</title>\r\n  </head>\r\n\r\n  <body>\r\n    <p>body 。</p>\r\n    <p>title 。</p>\r\n  </body>\r\n  <script>\r\n    window.onload = function() {\r\n      alert('123')\r\n    }\r\n  </script>\r\n</html>\r\n";
        }

        private string GetJpg()
        {
            return "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/wgARCAAEAAIDASIAAhEBAxEB/8QAFQABAQAAAAAAAAAAAAAAAAAAAAb/xAAVAQEBAAAAAAAAAAAAAAAAAAAEBf/aAAwDAQACEAMQAAABqRJX/8QAFBABAAAAAAAAAAAAAAAAAAAAAP/aAAgBAQABBQJ//8QAFBEBAAAAAAAAAAAAAAAAAAAAAP/aAAgBAwEBPwF//8QAFBEBAAAAAAAAAAAAAAAAAAAAAP/aAAgBAgEBPwF//8QAFBABAAAAAAAAAAAAAAAAAAAAAP/aAAgBAQAGPwJ//8QAFBABAAAAAAAAAAAAAAAAAAAAAP/aAAgBAQABPyF//9oADAMBAAIAAwAAABAD/8QAFBEBAAAAAAAAAAAAAAAAAAAAAP/aAAgBAwEBPxB//8QAFBEBAAAAAAAAAAAAAAAAAAAAAP/aAAgBAgEBPxB//8QAFBABAAAAAAAAAAAAAAAAAAAAAP/aAAgBAQABPxB//9k=";
        }

        private string GetJpeg()
        {
            return "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/wgARCAAEAAIDASIAAhEBAxEB/8QAFQABAQAAAAAAAAAAAAAAAAAAAAb/xAAVAQEBAAAAAAAAAAAAAAAAAAAEBf/aAAwDAQACEAMQAAABqRJX/8QAFBABAAAAAAAAAAAAAAAAAAAAAP/aAAgBAQABBQJ//8QAFBEBAAAAAAAAAAAAAAAAAAAAAP/aAAgBAwEBPwF//8QAFBEBAAAAAAAAAAAAAAAAAAAAAP/aAAgBAgEBPwF//8QAFBABAAAAAAAAAAAAAAAAAAAAAP/aAAgBAQAGPwJ//8QAFBABAAAAAAAAAAAAAAAAAAAAAP/aAAgBAQABPyF//9oADAMBAAIAAwAAABAD/8QAFBEBAAAAAAAAAAAAAAAAAAAAAP/aAAgBAwEBPxB//8QAFBEBAAAAAAAAAAAAAAAAAAAAAP/aAAgBAgEBPxB//8QAFBABAAAAAAAAAAAAAAAAAAAAAP/aAAgBAQABPxB//9k=";
        }

        private string GetPng()
        {
            return "iVBORw0KGgoAAAANSUhEUgAAAAMAAAACCAYAAACddGYaAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAASSURBVBhXY3jGIPQfhpE4Qv8BmEULyz97vc8AAAAASUVORK5CYII=";
        }

        private string GetTiff()
        {
            return "SUkqABoAAACAPVonsAQWBQSDQOCgCDwuAgAVAP4ABAABAAAAAAAAAAABBAABAAAAAgAAAAEBBAABAAAABAAAAAIBAwADAAAAHAEAAAMBAwABAAAABQAAAAYBAwABAAAAAgAAAA0BAgAMAAAAIgEAABEBBAABAAAACAAAABUBAwABAAAAAwAAABYBBAABAAAABAAAABcBBAABAAAAEQAAABoBBQABAAAALgEAABsBBQABAAAANgEAABwBAwABAAAAAQAAACgBAwABAAAAAgAAACkBAwACAAAAAAABADEBAgBHAAAAPgEAAD0BAwABAAAAAgAAAFMBAwADAAAAhgEAAFsBBwA+AgAAjAEAABQCBQAGAAAAygMAAAAAAAAIAAgACABvdXRwdXQudGlmZgAAdwEA6AMAAAB3AQDoAwAAR3JhcGhpY3NNYWdpY2sgMS40IHNuYXBzaG90LTIwMTkwNDIzIFE4IGh0dHA6Ly93d3cuR3JhcGhpY3NNYWdpY2sub3JnLwAAAQABAAEA/9j/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/xAAfAAABBQEBAQEBAQAAAAAAAAAAAQIDBAUGBwgJCgv/xAC1EAACAQMDAgQDBQUEBAAAAX0BAgMABBEFEiExQQYTUWEHInEUMoGRoQgjQrHBFVLR8CQzYnKCCQoWFxgZGiUmJygpKjQ1Njc4OTpDREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4eLj5OXm5+jp6vHy8/T19vf4+fr/xAAfAQADAQEBAQEBAQEBAAAAAAAAAQIDBAUGBwgJCgv/xAC1EQACAQIEBAMEBwUEBAABAncAAQIDEQQFITEGEkFRB2FxEyIygQgUQpGhscEJIzNS8BVictEKFiQ04SXxFxgZGiYnKCkqNTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqCg4SFhoeIiYqSk5SVlpeYmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2dri4+Tl5ufo6ery8/T19vf4+fr/2QAAAAABAAAA/wAAAAEAAACAAAAAAQAAAP8AAAABAAAAgAAAAAEAAAD/AAAAAQAAAA==";
        }

        private string GetGif()
        {
            return "R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw==";
        }
    }

    public class ProfileController
    {

    }

    public class PrivateObject
    {
        public PrivateObject(object obj)
        {

        }

        public object Invoke(string methodName, params object[] obj)
        {
            return string.Empty;
        }
    }
}
