import React, { useState } from 'react';
import { Link } from 'react-router-dom'
 
export function FirstRegisterForm ({firstCallback}) {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPass, setConfirmPass] = useState('');
    const [nextForm] = useState(2);

    const handleNext = (e) => {
        e.preventDefault();

        var form = document.querySelector('.needs-validation');
        form.classList.add('was-validated');

        var pass = document.getElementById('passwordInput')
        var confirm = document.getElementById('confirmPasswordInput')
        var passwordIsValid = true

        if (pass.value !== confirm.value)
        {
            pass.classList.add('is-invalid')
            confirm.classList.add('is-invalid')
            passwordIsValid = false
        }
        else
        {
            pass.classList.remove('is-invalid')
            confirm.classList.remove('is-invalid')
        }

        if (form.checkValidity() && passwordIsValid)
        {
            var data = {
                email: email,
                password: password,
                nextForm: nextForm
            }
            firstCallback(data)
        }
    }

    return (
        <form onSubmit={handleNext} className="needs-validation" noValidate>
              <div className="form-outline mb-4">
                <label className="form-label" htmlFor="emailInput">Email</label>
                <input value={email} onChange={(e) => setEmail(e.target.value)} type="email"
                id="emailInput" className="form-control" required/>
                <div className="invalid-feedback">
                    Please provide a valid email.
                </div>
              </div>
              

              <div className="form-outline mb-4">
                <label className="form-label" htmlFor="passwordInput">Password</label>
                <input value={password} onChange={(e) => setPassword(e.target.value)} type="password"
                id="passwordInput" className="form-control" required/>
              </div>

              <div className="form-outline mb-4">
                <label className="form-label" htmlFor="confirmPasswordInput">Confirm password</label>
                <input value={confirmPass} onChange={(e) => setConfirmPass(e.target.value)} type="password"
                id="confirmPasswordInput" className="form-control" required/>
                <div className="invalid-feedback">
                    The two passwords must match.
                </div>
              </div>

              <div className="row container-fluid">
              <button type="submit" className="col-sm-3 btn btn-primary btn-block" style={{backgroundColor: "#3474b0"}}>
                Next
              </button>
              <span className="col-sm-8 pt-2">Already have an account? <Link to="/login">Sign in</Link></span>
              </div>
            </form>
    )
}