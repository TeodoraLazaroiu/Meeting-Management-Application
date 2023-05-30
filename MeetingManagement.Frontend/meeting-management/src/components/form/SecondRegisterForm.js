import React, { useState } from 'react';
import { Link } from 'react-router-dom'
 
export function SecondRegisterForm ({secondCallback}) {
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [roleTitle, setRoleTitle] = useState('');
    const [nextForm] = useState(null);

    const handleSubmit = (e) => {
        e.preventDefault();

        var form = document.querySelector('.needs-validation');
        form.classList.add('was-validated');

        if (form.checkValidity())
        {
          console.log(firstName)
          var data = {
            firstName: firstName,
            lastName: lastName,
            roleTitle: roleTitle,
            nextForm: nextForm
          }
          secondCallback(data)
        }
    }

    const handleBack = () => {
      var data = {
        firstName: '',
        lastName: '',
        roleTitle: '',
        nextForm: 1
      }
      
      secondCallback(data)
    }

    return (
        <form onSubmit={handleSubmit} className="needs-validation" noValidate>
              <div className="form-outline mb-4">
                <label className="form-label" htmlFor="firstNameInput">First Name</label>
                <input value={firstName} onChange={(e) => setFirstName(e.target.value)} type="text"
                id="firstNameInput" className="form-control" required/>
              </div>
              

              <div className="form-outline mb-4">
                <label className="form-label" htmlFor="lastNameInput">Last Name</label>
                <input value={lastName} onChange={(e) => setLastName(e.target.value)} type="text"
                id="lastNameInput" className="form-control" required/>
              </div>

              <div className="form-outline mb-4">
                <label className="form-label" htmlFor="roleTitleInput">Role Title</label>
                <input value={roleTitle} onChange={(e) => setRoleTitle(e.target.value)} type="text"
                id="roleTitleInput" className="form-control"/>
              </div>

              <div className="row container-fluid">
              <button onClick={handleBack} type="button" className="mx-2 col-sm-3 btn btn-secondary btn-block">
                Back
              </button>
              <button type="submit" className="col-sm-3 btn btn-primary btn-block">Submit
              </button>
              <span className="col-sm-3 pt-2"><Link to="/login">Sign in</Link></span>
              </div>
            </form>
    )
}