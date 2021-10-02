using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Net.Mime;
using System.Text;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using WebLib;
using WebLib.Models;
using WebLib.AspNet;

namespace WebDesk
{

    /// <summary>
    /// Helper for JWT token authentication.
    /// </summary>
    static internal class JwtAuthHelper
    {
 
        /// <summary>
        /// Decodes a specified token.
        /// </summary>
        static void DecodeToken(JwtSecurityToken Token)
        {
            //Claim Claim = Token.Claims.FirstOrDefault(item => item.Type == "A TOKEN TYPE HERE");
            //if (Claim != null)
            //{                
            //}
        }
        /// <summary>
        /// Event handler. The <see cref="JwtBearerEvents.OnMessageReceived"/> event gives the application an opportunity to get the token from a different location, adjust, or reject the token.
        /// <para>The application may set the Context.Token in the OnMessageReceived. Otherwise Context.Token is null.</para>
        /// <para>SEE: https://stackoverflow.com/a/54497616/1779320        </para>
        /// </summary>
        static Task MessageReceived(MessageReceivedContext Context)
        {
            // So, if we need the token here in this event, we may use
            // JwtSecurityToken Token = ReadTokenFromRequestHeader(Context.HttpContext);  

            return Task.CompletedTask;
        }
        /// <summary>
        /// Event handler. The <see cref="JwtBearerEvents.OnTokenValidated" /> is called after the passed in <see cref="TokenValidatedContext.SecurityToken"/> is loaded and validated successfully.
        /// </summary>
        static Task TokenValidated(TokenValidatedContext Context)
        {
            // Here we may use the following
            // JwtSecurityToken Token = Context.SecurityToken as JwtSecurityToken;
            // if (Token != null)
            //    DecodeToken(Token);

            return Task.CompletedTask;
        }

        /* public */
        /// <summary>
        /// For debug purposes. 
        /// Reads a token by reading the <see cref="HeaderNames.Authorization"/> header 
        /// from <see cref="HttpRequest.Headers"/>, 
        /// and converting the token string to a <see cref="JwtSecurityToken"/>
        /// </summary>
        static public JwtSecurityToken ReadTokenFromRequestHeader(HttpContext HttpContext)
        {
            // here we get the token from HTTP headers
            string sToken = WSys.GetHttpHeader(HttpContext.Request, HeaderNames.Authorization);

            if (!string.IsNullOrWhiteSpace(sToken))
            {
                sToken = sToken.Replace("Bearer ", string.Empty);
                JwtSecurityToken Token = new JwtSecurityTokenHandler().ReadJwtToken(sToken);
                return Token;
            }

            return null;
        }
        /// <summary>
        /// Configures the <see cref="JwtBearerOptions"/>. To be called from Startup.
        /// </summary>
        static public void SetJwtBearerConfigurationOptions(JwtBearerOptions o, JwtSettings Settings)
        {
            TokenValidationParameters ValidationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = Settings.Issuer,

                ValidateAudience = true,
                ValidAudiences = new List<string> {
                            Settings.Audience
                        },

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.Secret)),

                ValidateLifetime = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero
            };


            o.RequireHttpsMetadata = false;
            o.SaveToken = true;
            o.TokenValidationParameters = ValidationParams;

            //o.Events = new JwtBearerEvents();
            //o.Events.OnMessageReceived = MessageReceived;
            //o.Events.OnTokenValidated = TokenValidated;
        }
        /// <summary>
        /// Creates and returns a JWT authenticated result (along with token and claims)
        /// </summary>
        static public OkObjectResult GetAuthenticatedResult(IClaimListProvider ClaimListProvider, JwtSettings Settings, string CultureCode)
        {
            // claims
            List<Claim> ClaimList = ClaimListProvider.GetClaimList(Lib.JwtAuthScheme);

            // private claims
            ClaimList.Add(new Claim(Requestor.SCultureClaimType, CultureCode));

            // create the JWT token
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.Secret));
            JwtSecurityToken Token = new JwtSecurityToken(
                issuer: Settings.Issuer,
                audience: Settings.Audience,
                claims: ClaimList.ToArray(),
                expires: DateTime.UtcNow.AddMinutes(Settings.LifeTimeMinutes),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256)
            );

            // return result
            OkObjectResult Result = new OkObjectResult(new
            {
                access_token = new JwtSecurityTokenHandler().WriteToken(Token),
                expires_in = (int)Token.ValidTo.Subtract(DateTime.UtcNow).TotalSeconds,
                //sub = loginViewModel.Username,
                //name = loginViewModel.Username,
                //fullName = user.FullName,
                //jobtitle = string.Empty,
                //phone = string.Empty,
                //email = user.EmailName,
            });

            return Result;
        }
    }
}
