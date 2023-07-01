import React, { useContext, useState, useEffect } from 'react';
import { Link } from 'react-router-dom'
import axios from 'axios';
import AuthContext from '../../utils/AuthContext';
import { toast } from 'react-toastify';

export const Navbar = () => {
    const [userName, setUserName] = useState('');
    const apiUrl = process.env.REACT_APP_API_URL
    const client = axios.create({
      withCredentials: true,
      baseURL: apiUrl
    })

    const handleLogout = async () => {
        await client.post('/auth/signOut')
          .then(() => {
            setAuthenticated(false)
          })
          .catch((error) => {
            console.log(error.response.data)
          });
    }

    useEffect(() => {
      client.get('/user')
          .then((response) => {
            console.log(response.data)
            setUserName(response.data.firstName + " " + response.data.lastName)
          })
          .catch((error) => {
            console.log(error)
          });
    }, []);

    const { setAuthenticated } = useContext(AuthContext);

    return (
    <nav className="navbar navbar-expand-lg navbar-light bg-light border-bottom box-shadow" style={{maxHeight: "60px"}}>
        <Link className="text-lg-start navbar-brand" to="/home">
        <h5 className="mx-3 fw-bold">
            Meeting <br/>
            <span className="" style={{color: "#3474b0"}}>Management</span>
            </h5>
        </Link>
        <div className="navbar-nav">
            <Link className="nav-item nav-link active" to="/home">Home</Link>
            <Link className="nav-item nav-link" to="/team">Team</Link>
            <Link onClick={handleLogout} className="nav-item nav-link" to="/login">Logout</Link>
        </div>
        <div className="ms-auto mx-5">
        <Link className="navbar-text nav-item nav-link" to="/home">Welcome, {userName}</Link>
        </div>
    </nav>
    )
}