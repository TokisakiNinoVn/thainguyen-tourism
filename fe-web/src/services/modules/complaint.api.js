import instance from '@/services/axiosConfig';

// Get all complaint
const getAll = async () => instance.get('api/private/complaints/');


export {
  getAll,
};
