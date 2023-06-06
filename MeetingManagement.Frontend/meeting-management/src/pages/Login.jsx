import React, { useState, useContext, useEffect} from 'react';
import { Link, useNavigate } from 'react-router-dom'
import { WelcomeMessage } from '../components/ui/WelcomeMessage'
import axios from 'axios';
import AuthContext from '../utils/AuthContext';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

export const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const apiUrl = process.env.REACT_APP_API_URL
    const client = axios.create({
      withCredentials: true,
      baseURL: apiUrl
    })

    const navigate = useNavigate();
    const [isSuccessful, setIsSuccessful] = useState(false);
    const { setAuthenticated } = useContext(AuthContext);

    const handleSubmit = async (e) => {
        e.preventDefault();

        var form = document.querySelector('.needs-validation');
        form.classList.add('was-validated');

        if (form.checkValidity())
        {
            await client.post('/auth/signIn', {
              "email": email,
              "password": password
            })
            .then((response) => {
              setIsSuccessful(true);
            })
            .catch((error) => {
              toast.error(error.response.data)
            });
        }
    }

    useEffect(() => {
      if (isSuccessful) {
        setAuthenticated(true);
        navigate('/home')
      }
    })

    const styles = {
      main : {
        backgroundColor: "rgb(240,240,240)",
        paddingTop: "120px",
        height: "100vh"
      }
    }
     
    return (
        <div className="container-fluid" style={styles.main}>
          <div className="apx-4 py-5 px-md-5 text-lg-start">
              <div className="row gx-lg-5">
                <WelcomeMessage/>
        
      <div className="col-lg-6 mb-5 mb-lg-0">
        <div className="card">
          <div className="card-body py-5 px-md-5">
            <form onSubmit={handleSubmit} className="needs-validation" noValidate>
              <div className="form-outline mb-4">
                <label className="form-label" htmlFor="emailInput">Email</label>
                <input value={email} onChange={(e) => setEmail(e.target.value)} type="email" id="emailInput" className="form-control" required/>
              </div>

              <div className="form-outline mb-4">
                <label className="form-label" htmlFor="passwordInput">Password</label>
                <input value={password} onChange={(e) => setPassword(e.target.value)} type="password" id="passwordInput" className="form-control" required/>
              </div>

              <div className="row container-fluid">
              <button type="submit" className="col-sm-3 btn btn-secondary btn-block" style={{backgroundColor: "#3474b0"}}>
                Sign in
              </button>
              <span className="col-sm-9 px-3 pt-2">Don't have an account? <Link to="/register">Sign up</Link></span>
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