namespace Billi.Backend.CrossCutting.Utilities
{
    public static class EmailTemplates
    {
        private static readonly string _resetLink = "https://billi.shop/reset-password";

        public static string GetPasswordUpdatedEmail()
        {
            return @"
                <!DOCTYPE html>
                <html lang=""pt-BR"">
                <head>
                    <meta charset=""UTF-8"">
                    <title>Senha Atualizada</title>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            background-color: #f9fafb;
                            color: #333;
                            margin: 0;
                            padding: 20px;
                        }

                        .container {
                            background-color: #ffffff;
                            border-radius: 8px;
                            padding: 30px;
                            max-width: 600px;
                            margin: auto;
                            box-shadow: 0 4px 6px rgba(0,0,0,0.1);
                        }

                        .title {
                            color: #22c55e;
                            font-size: 20px;
                            font-weight: bold;
                            margin-bottom: 20px;
                        }

                        .content {
                            font-size: 16px;
                            line-height: 1.6;
                        }

                        .footer {
                            margin-top: 30px;
                            font-size: 12px;
                            color: #999;
                            text-align: center;
                        }
                    </style>
                </head>
                <body>
                    <div class=""container"">
                        <div class=""title"">Senha alterada com sucesso!</div>
                        <div class=""content"">
                            <p>Olá,</p>
                            <p>Informamos que sua senha foi atualizada com sucesso.</p>
                            <p>Se você não realizou essa alteração, por favor entre em contato imediatamente com nosso suporte.</p>
                            <p>Agradecemos pela sua confiança.</p>
                        </div>
                        <div class=""footer"">
                            © 2025 Billishop. Todos os direitos reservados.
                        </div>
                    </div>
                </body>
                </html>
                ";
        }

        public static string GenerateResetPasswordEmail(string code)
        {
            return $@"
                <!DOCTYPE html>
                <html lang=""pt-BR"">
                <head>
                    <meta charset=""UTF-8"">
                    <title>Redefinição de Senha</title>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f6f6f6;
                            padding: 20px;
                            color: #333;
                        }}
                        .container {{
                            background-color: #ffffff;
                            padding: 30px;
                            border-radius: 8px;
                            max-width: 600px;
                            margin: auto;
                            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
                        }}
                        .code {{
                            font-size: 24px;
                            font-weight: bold;
                            background-color: #efefef;
                            padding: 10px 20px;
                            border-radius: 5px;
                            display: inline-block;
                            margin: 20px 0;
                            letter-spacing: 2px;
                        }}
                        .link {{
                            display: inline-block;
                            margin-top: 20px;
                            text-decoration: none;
                            background-color: #007bff;
                            color: white;
                            padding: 12px 20px;
                            border-radius: 5px;
                            font-weight: bold;
                        }}
                        .footer {{
                            margin-top: 30px;
                            font-size: 12px;
                            color: #888;
                        }}
                    </style>
                </head>
                <body>
                    <div class=""container"">
                        <h2>Redefinição de Senha</h2>
                        <p>Você solicitou a redefinição da sua senha na <strong>BilliShop</strong>.</p>
                        <p>Use o código abaixo para continuar com o processo:</p>

                        <div class=""code"">{code}</div>

                        <p>Ou clique no botão abaixo para redefinir diretamente:</p>
                        <a href=""{_resetLink}"" class=""link"">Redefinir Senha</a>

                        <p class=""footer"">
                            Se você não solicitou essa alteração, apenas ignore este e-mail.
                        </p>
                    </div>
                </body>
                </html>";
        }
    }
}
