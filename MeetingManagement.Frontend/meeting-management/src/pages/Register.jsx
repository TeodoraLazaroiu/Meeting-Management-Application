import axios from 'axios';
import React, { useEffect, useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { WelcomeMessage } from '../components/ui/WelcomeMessage'
import { FirstRegisterForm } from '../components/form/FirstRegisterForm'
import { SecondRegisterForm } from '../components/form/SecondRegisterForm'
import AuthContext from '../utils/AuthContext';

export const Register = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [roleTitle, setRoleTitle] = useState('');
    const [formNumber, setFormNumber] = useState(1);
    const [registerSuccess, setRegisterSuccess] = useState(false);

    const apiUrl = process.env.REACT_APP_API_URL
    const client = axios.create({
      withCredentials: true,
      baseURL: apiUrl
    })

    const styles = {
        main : {
          backgroundColor: "#F0F0F0",
          paddingTop: "120px",
          height: "100vh"
        }
      }

    const firstCallback = data => {
        setEmail(data.email)
        setPassword(data.password)
        setFormNumber(data.nextForm)
    }
    
    const secondCallback = (data) => {
        setFirstName(data.firstName)
        setLastName(data.lastName)
        setRoleTitle(data.roleTitle)
        setFormNumber(data.nextForm)
    }

    useEffect(() => {
      if (formNumber === null) {
        registerUser();
      }
    }, [formNumber])

    const registerUser = async () => {
      await client.post('/user', {
          "email": email,
          "password": password,
          "firstName": firstName,
          "lastName": lastName,
          "roleTitle": roleTitle
        })
        .then(() => {
          setRegisterSuccess(true)
          toast.success('Account created successfully')
        })
        .catch((error) => {
          toast.error(error.response.data)
          console.log(error.response.data)
        });
    }

    const navigate = useNavigate()
    const { setAuthenticated } = useContext(AuthContext);

    useEffect(() => {
      if (registerSuccess) loginUser()
    }, [registerSuccess]);

    const loginUser = async () => {
      await client.post('/auth/signIn', {
          "email": email,
          "password": password
        })
        .then(() => {
          setAuthenticated(true);
          navigate('/join')
        })
        .catch((error) => {
          toast.error(error.response.data)
        });
    }

    return (
        <div className="container-fluid" style={styles.main}>
        <div className="apx-4 py-5 px-md-5 text-lg-start">
            <div className="row gx-lg-5">
                <WelcomeMessage/>
        
    <div className="col-lg-6 mb-5 mb-lg-0">
        <div className="card">
        <div className="card-body py-5 px-md-5">
            {formNumber === 1 && <FirstRegisterForm firstCallback={firstCallback}/>}
            {formNumber === 2 && <SecondRegisterForm secondCallback={secondCallback}/>}
            {formNumber === null && <SecondRegisterForm secondCallback={secondCallback}/>}
        </div>
        </div>
    </div>
            </div>
            </div>
        </div>
    )
}