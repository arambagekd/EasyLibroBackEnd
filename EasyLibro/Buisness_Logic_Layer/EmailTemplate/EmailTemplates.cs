using Data_Access_Layer.Entities;
using MimeKit;
using MimeKit.Text;

namespace Buisness_Logic_Layer.EmailTemplates
{
    public class EmailTemplate
    {
        public TextPart IssueBookEmail(Reservation reservation, string userName, Resource Book)
        {
            var htmlBody = new TextPart(TextFormat.Html)
            {
                Text = $@"
                <!DOCTYPE html>
                <html lang=""en"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>Reservation Details</title>
                    <style>
                        /* Reset styles */
                        body, html {{
                            margin: 0;
                            padding: 0;
                            font-family: Arial, sans-serif;
                            background-color: #f8f8f8;
                        }}
                        /* Container */
                        .container {{
                            width: 100%;
                            padding: 20px;
                        }}
                        /* Content box */
                        .content {{
                            max-width: 600px;
                            margin: 0 auto;
                            background-color: #ffffff;
                            border-radius: 8px;
                            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                        }}
                        /* Header */
                        .header {{
                            padding: 20px 40px;
                            text-align: center;
                            background-color: #007bff;
                            color: #ffffff;
                            border-top-left-radius: 8px;
                            border-top-right-radius: 8px;
                        }}
                        .header h1 {{
                            margin: 0;
                            font-size: 24px;
                        }}
                        /* Details */
                        .details {{
                            display: flex;
                            border: 1px solid #e0e0e0;  
                            padding: 40px;
                            color: #333333;
                            font-size: 16px;
                            line-height: 1.5;
                            justify-content: space-between; /* Align items evenly */
                        }}
                        .details .left {{
                            flex: 1; /* Take up remaining space */
                        }}
                        .details .right {{
                            flex-shrink: 0; /* Don't allow to shrink */
                            margin-left: 80px; /* Add some space between left and right */
                        }}
                        .details p {{
                            margin: 10px 0;
                        }}
                        /* Visit button */
                        .visit-btn {{
                            padding: 10px 40px;
                            text-align: center;
                            background-color: #007bff;
                            color: #ffffff;
                            text-decoration: none;
                            border-bottom-left-radius: 8px;
                            border-bottom-right-radius: 8px;
                            font-weight: bold;
                            display: block;
                        }}
                    </style>
                </head>
                <body>
                    <div class=""container"">
                        <div class=""content"">
                            <p>Dear Patron {userName},</p>
                            <p>We are delighted to provide you with the details of your reservation. Your support is greatly appreciated, and we hope this reservation enhances your experience with our services. Should you have any questions or need further assistance, please feel free to reach out to us. Thank you for choosing us!</p>
                            <div class=""header"">
                                <h1>Reservation Details</h1>
                            </div>
                            <div class=""details"">
                                <div class=""left"">
                                    <p><strong>Reservation ID:</strong> {reservation.ReservationNo}</p>
                                    <p><strong>ISBN:</strong> {reservation.ResourceId}</p>
                                    <p><strong>Book Name:</strong> {Book.Title}</p>
                                    <p><strong>Due Date:</strong> {reservation.DueDate}</p>
                                    <p><strong>Issue Date:</strong> {reservation.IssuedDate}</p>
                                    <p><strong>Returned Date:</strong> {reservation.ReturnDate}</p>
                                </div>
                                <div class=""right"">
                                    <img src={Book.ImageURL} alt=""Book Image"" style=""width:140px;height:200px;"">
                                </div>
                            </div>
                            <a href=""https://easylibro.online"" ><div class=""visit-btn"">Go to Easylibro</div></a>
                        </div>
                    </div>
                </body>
                </html>"
            };
            return htmlBody;
        }
        public TextPart ReturnBookEmail(Reservation reservation, string userName, Resource Book)
        {
            var htmlBody = new TextPart(TextFormat.Html)
            {
                Text = $@"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Return Confirmation</title>
            <style>
                /* Reset styles */
                body, html {{
                    margin: 0;
                    padding: 0;
                    font-family: Arial, sans-serif;
                    background-color: #f8f8f8;
                }}
                /* Container */
                .container {{
                    width: 100%;
                    padding: 20px;
                }}
                /* Content box */
                .content {{
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    border-radius: 8px;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                }}
                /* Header */
                .header {{
                    padding: 20px 40px;
                    text-align: center;
                    background-color: #28a745;
                    color: #ffffff;
                    border-top-left-radius: 8px;
                    border-top-right-radius: 8px;
                }}
                .header h1 {{
                    margin: 0;
                    font-size: 24px;
                }}
                /* Details */
                .details {{
                    display: flex;
                    border: 1px solid #e0e0e0;
                    padding: 40px;
                    color: #333333;
                    font-size: 16px;
                    line-height: 1.5;
                    justify-content: space-between; /* Align items evenly */
                }}
                .details .left {{
                    flex: 1; /* Take up remaining space */
                }}
                .details .right {{
                    flex-shrink: 0; /* Don't allow to shrink */
                    margin-left: 80px; /* Add some space between left and right */
                }}
                .details p {{
                    margin: 10px 0;
                }}
                /* Visit button */
                .visit-btn {{
                    padding: 10px 40px;
                    text-align: center;
                    background-color: #28a745;
                    color: #ffffff;
                    text-decoration: none;
                    border-bottom-left-radius: 8px;
                    border-bottom-right-radius: 8px;
                    font-weight: bold;
                    display: block;
                }}
            </style>
        </head>
        <body>
            <div class=""container"">
                <div class=""content"">
                    <p>Dear {userName},</p>
                    <p>We are pleased to confirm that the book you borrowed has been successfully returned. Thank you for using our services. We look forward to serving you again in the future. Should you have any questions or need further assistance, please feel free to reach out to us.</p>
                    <div class=""header"">
                        <h1>Return Confirmation</h1>
                    </div>
                    <div class=""details"">
                        <div class=""left"">
                            <p><strong>Reservation ID:</strong> {reservation.ReservationNo}</p>
                            <p><strong>ISBN:</strong> {reservation.ResourceId}</p>
                            <p><strong>Book Name:</strong> {(Book != null ? Book.Title : "Book removed")}</p>
                            <p><strong>Due Date:</strong> {reservation.DueDate}</p>
                            <p><strong>Issue Date:</strong> {reservation.IssuedDate}</p>
                            <p><strong>Returned Date:</strong> {reservation.ReturnDate}</p>
                        </div>
                        <div class=""right"">
                            <img src=""{(Book!=null?Book.ImageURL:"")}"" alt=""Book Image"" style=""width:140px;height:200px;"">
                        </div>
                    </div>
                    <a href=""https://easylibro.online"" ><div class=""visit-btn"">Go to Easylibro</div></a>
                </div>
            </div>
        </body>
        </html>"
            };
            return htmlBody;
        }
        public TextPart DefaultPassword(string password)
        {
            var htmlBody = new TextPart(TextFormat.Html)
            {
                Text = $@"
                <!DOCTYPE html>
                        <html lang=""en"">
                        <head>
                        <meta charset=""UTF-8"">
                        <title>Welcome to Our Site!</title>
                        <style>
                          body {{
                            font-family: Arial, sans-serif;
                            margin: 0;
                            padding: 0;
                            background-color: #f4f4f4;
                            color: #333;
                          }}
                          .container {{
                            max-width: 600px;
                            margin: 20px auto;
                            padding: 20px;
                            background-color: #fff;
                            border-radius: 8px;
                            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
                          }}
                          h1 {{
                            color: #007bff;
                          }}
                          ul {{
                            list-style-type: none;
                            padding: 0;
                          }}
                          ul li {{
                            margin-bottom: 10px;
                          }}
                          p {{
                            line-height: 1.6;
                          }}
                          footer {{
                            margin-top: 20px;
                            text-align: center;
                            color: #888;
                          }}
                        </style>
                        </head>
                        <body>

                          <div class=""container"">
                            <h1>Welcome to Our Site!</h1>

                            <p>Hello Easy Libro User,</p>

                            <p>Welcome to our website! We are excited to have you on board. As you are logging in for the first time, here are your password :</p>

                            <ul>
                              <li><strong>Default Password:</strong> {password}</li>
                            </ul>

                            <p>Please make sure to change your password after logging in for the first time to ensure the security of your account.</p>

                            <p>If you have any questions or need assistance, feel free to contact our support team at <a href=""""mailto:easylibro@gmail.com"""">easylibro@gmail.com</a>.</p>

                            <footer>
                              Best regards,<br>
                              EasyLibro
                            </footer>
                          </div>

                        </body>
                        </html>

               "
            };


            return htmlBody;
        }
        public TextPart ResetPassword(string link)
        {
            var htmlBody = new TextPart(TextFormat.Html)
            {
                Text = $@"
                <!DOCTYPE html>
                        <html lang=""en"">
                        <head>
                        <meta charset=""UTF-8"">
                        <title>Welcome to Our Site!</title>
                        <style>
                          body {{
                            font-family: Arial, sans-serif;
                            margin: 0;
                            padding: 0;
                            background-color: #f4f4f4;
                            color: #333;
                          }}
                          .container {{
                            max-width: 600px;
                            margin: 20px auto;
                            padding: 20px;
                            background-color: #fff;
                            border-radius: 8px;
                            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
                          }}
                          h1 {{
                            color: #007bff;
                          }}
                          ul {{
                            list-style-type: none;
                            padding: 0;
                          }}
                          ul li {{
                            margin-bottom: 10px;
                          }}
                          p {{
                            line-height: 1.6;
                          }}
                          footer {{
                            margin-top: 20px;
                            text-align: center;
                            color: #888;
                          }}
                        </style>
                        </head>
                        <body>

                          <div class=""container"">
                            <h1>Reset Your Password</h1>

                            <p>Hello Easy Libro User,</p>

                            <p>Using below link you can reset you password...</p>

                            <ul>
                              <li>Reset your password by <a href='{link}'>clicking here</a>.</li>
                            </ul>

                            <p>Please make sure to change your password after logging in for the first time to ensure the security of your account.</p>

                            <p>If you have any questions or need assistance, feel free to contact our support team at <a href=""""mailto:easylibro@gmail.com"""">easylibro@gmail.com</a>.</p>

                            <footer>
                              Best regards,<br>
                              EasyLibro
                            </footer>
                          </div>

                        </body>
                        </html>

               "
            };


            return htmlBody;
        }
        public TextPart RemindEmail(Reservation reservation)
        {
            var htmlBody = new TextPart(TextFormat.Html)
            {
               Text = $@"
                                <!DOCTYPE html>
                                <html lang=""en"">
                                <head>
                                <meta charset=""UTF-8"">
                                <title>Library Notification: Overdue Book</title>
                                <style>
                                    body {{
                                        font-family: Arial, sans-serif;
                                        margin: 0;
                                        padding: 0;
                                        background-color: #f4f4f4;
                                        color: #333;
                                    }}
                                    .container {{
                                        max-width: 600px;
                                        margin: 20px auto;
                                        padding: 20px;
                                        background-color: #fff;
                                        border-radius: 8px;
                                        box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
                                    }}
                                    h1 {{
                                        color: #007bff;
                                    }}
                                    ul {{
                                        list-style-type: none;
                                        padding: 0;
                                    }}
                                    ul li {{
                                        margin-bottom: 10px;
                                    }}
                                    p {{
                                        line-height: 1.6;
                                    }}
                                    footer {{
                                        margin-top: 20px;
                                        text-align: center;
                                        color: #888;
                                    }}
                                </style>
                                </head>
                                <body>

                                <div class=""container"">
                                    <h1>Library Notification: Overdue Book</h1>

                                    <p>Hello,</p>

                                    <p>This is a reminder that the book reserved with Reservation ID: <strong>{reservation.Id}</strong> is overdue.</p>

                                    <ul>
                                        <li><strong>Book ISBN:</strong> {reservation.ResourceId}</li>
                                        <li><strong>Overdue Date:</strong> {reservation.DueDate}</li>
                                    </ul>

                                    <p>Please return the book as soon as possible to avoid any fines.</p>

                                    <p>If you have any questions or need assistance, feel free to contact our support team at <a href=""mailto:easylibro@gmail.com"">easylibro@gmail.com</a>.</p>

                                    <footer>
                                        Best regards,<br>
                                        Your Library
                                    </footer>
                                </div>

                                </body>
                                </html>"

        };


            return htmlBody;
        }
    }
}
