﻿using System;
using System.Threading.Tasks;

namespace QuicKE.Client.Tests
{
    public class MockRegisterServiceProxy : IRegisterServiceProxy
    {
        public Task<RegisterResult> RegisterAsync(string username, string email, string password, string confirm)
        {
            //// create a task that simulates a call up to the server...
            //return Task.Factory.StartNew(() =>
            //{
            //    // validate the data...
            //    ErrorBucket errors = new ErrorBucket();
            //    if (string.IsNullOrEmpty(username))
            //        errors.AddError("Username is required.");
            //    if (string.IsNullOrEmpty(email))
            //        errors.AddError("Emailis required.");
            //    if (string.IsNullOrEmpty(password))
            //        errors.AddError("Password is required.");
            //    if (string.IsNullOrEmpty(confirm))
            //        errors.AddError("Confirm password is required.");

            //    // match?
            //    if (!(string.IsNullOrEmpty(password)) && password != confirm)
            //        errors.AddError("The passwords do not match.");

            //    // if?
            //    if(!(errors.HasErrors))
            //    {
            //        // raise a success result...
            //        RegisterResult result = new RegisterResult(Guid.NewGuid().ToString());
            //        success(result);

            //        // complete?
            //        if (complete != null)
            //            complete();
            //    }

            //    // fail?
            //    if(errors.HasErrors)
            //        failure(this, errors);

            //});

            throw new NotImplementedException("This operation has not been implemented.");
        }
    }
}
