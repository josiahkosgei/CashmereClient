
//Security.Authentication.LDAP.LDAPAuthentication


using System;
using System.DirectoryServices.Protocols;
using System.Net;

namespace CashmereUtil.Security.Authentication.LDAP
{
    public class LDAPAuthentication
    {
        public string Host { get; set; }

        public bool UseSSL { get; set; }

        public bool IgnoreCert { get; set; }

        public string Domain { get; set; }

        public NetworkCredential Credential { get; set; }

        public LDAPAuthentication(
          string host,
          string user_search_domain = "",
          bool use_ssl = false,
          bool ignore_cert = true,
          NetworkCredential networkCredential = null)
        {
            Host = host;
            UseSSL = use_ssl;
            IgnoreCert = ignore_cert;
            Domain = user_search_domain;
            Credential = networkCredential;
        }

        public LdapConnection SetupConnection(NetworkCredential networkCredential)
        {
            try
            {
                LdapConnection ldapConnection = new LdapConnection(Host);
                if (!UseSSL)
                {
                    ldapConnection.SessionOptions.SecureSocketLayer = UseSSL;
                    ldapConnection.AuthType = AuthType.Basic;
                }
                else
                    ldapConnection.AuthType = AuthType.Negotiate;
                if (IgnoreCert)
                    ldapConnection.SessionOptions.VerifyServerCertificate += (VerifyServerCertificateCallback)((_param1, _param2) => true);
                if (networkCredential != null)
                    ldapConnection.Credential = networkCredential;
                return ldapConnection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public SearchResponse Search(
          string searchFilter,
          NetworkCredential nc,
          string[] attributesToReturn = null)
        {
            using LdapConnection ldapConnection = SetupConnection(nc);
            SearchRequest searchRequest = new SearchRequest(Domain, searchFilter, SearchScope.Subtree, null);
            return ldapConnection.SendRequest(searchRequest) as SearchResponse;
        }

        public bool ActiveDirectoryAuthentication(string username, string password)
        {
            try
            {
                SetupConnection(new NetworkCredential(username, password)).Bind();
                return true;
            }
            catch (LdapException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
            }
            return false;
        }
    }
}
