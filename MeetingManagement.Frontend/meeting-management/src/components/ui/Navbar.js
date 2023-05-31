import React, { useContext } from 'react';
import { Link } from 'react-router-dom'
import axios from 'axios';
import AuthContext from '../../utils/AuthContext';
import { toast } from 'react-toastify';

export const Navbar = () => {
    const apiUrl = process.env.REACT_APP_API_URL
    const client = axios.create({
      withCredentials: true,
      baseURL: apiUrl
    })

    const handleLogout = async () => {
        await client.post('/auth/signOut')
          .then((response) => {
            setAuthenticated(false)
          })
          .catch((error) => {
            toast.error(error.response.data)
          });
    }

    const { setAuthenticated } = useContext(AuthContext);

    return (
    <nav className="navbar navbar-expand-lg navbar-light bg-light border-bottom box-shadow" style={{maxHeight: "60px"}}>
        <Link className="text-lg-start navbar-brand" to="/home">
        <h5 className="mx-3 fw-bold">
            Meeting <br/>
            <span className="" style={{color: "#3474b0"}}>Management</span>
            </h5>
        </Link>
        <div className="text-lg-start navbar-nav">
            <Link className="nav-item nav-link active" to="/home">Home</Link>
            <Link className="nav-item nav-link" to="">Team</Link>
            <Link onClick={handleLogout} className="nav-item nav-link" to="/login">Logout</Link>
        </div>
    </nav>
    )
}