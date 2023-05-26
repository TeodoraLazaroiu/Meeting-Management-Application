import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom'
import { WelcomeMessage } from '../../components/ui/WelcomeMessage'

export const Register = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const styles = {
        main : {
          backgroundColor: "#F0F0F0",
          paddingTop: "120px",
          height: "100vh"
        }
      }

    const handleSubmit = (e) => {
        e.preventDefault();
    }
     
    return (
        <div className="container-fluid" style={styles.main}>
          <div className="apx-4 py-5 px-md-5 text-lg-start">
              <div className="row gx-lg-5">
                <WelcomeMessage/>
        
      <div className="col-lg-6 mb-5 mb-lg-0">
        <div className="card">
          <div className="card-body py-5 px-md-5">
            <form onSubmit={handleSubmit}>
              <div className="form-outline mb-4">
                <label className="form-label" htmlFor="emailInput">Email</label>
                <input value={email} onChange={(e) => setEmail(e.target.value)} type="email" id="emailInput" className="form-control" />
              </div>

              <div className="form-outline mb-4">
                <label className="form-label" htmlFor="passwordInput">Password</label>
                <input value={password} onChange={(e) => setPassword(e.target.value)} type="password" id="passwordInput" className="form-control" />
              </div>

              <div className="row container-fluid">
              <button type="submit" className="col-sm-3 btn btn-primary btn-block">
                Next
              </button>
              <span className="col-sm-9 px-3 pt-2">Already have an account? <Link to="/login">Sign in</Link></span>
              </div>
            </form>
          </div>
        </div>
      </div>
              </div>
            </div>
        </div>
    )
}