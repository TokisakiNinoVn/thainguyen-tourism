import axios from 'axios';

const instance = axios.create({
  baseURL: 'http://localhost:5124',
  headers: {
    'Content-Type': 'application/json',
  },
  withCredentials: true,
});

instance.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

instance.interceptors.response.use(
  (response) => {
    // Nếu dữ liệu trả về theo định dạng { data: { ... } }
    if (response.data && response.data.data) {
      return response.data;
    }
    return response.data;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default instance;
