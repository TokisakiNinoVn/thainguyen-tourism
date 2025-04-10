// auth.api.js
import instance from '@/services/axiosConfig';

// Get all users
const getAll = async () => instance.get('/api/private/customers');
const getAllBasic = async () => instance.get('/api/private/customers/basic');

// Add a new user
const add = async (data) =>
  instance.post('/api/private/customers/add', data);

// Get a specific user by ID
const getById = async (id) => instance.get(`/api/private/customers/${id}`);

// Update a user by ID
const update = async (id, { name, phone, address, gender, note }) =>
  instance.put(`/api/private/customers/${id}`, { name, phone, address, gender, note });

// Delete a user by ID
const remove = async (id) => instance.delete(`/api/private/customers/${id}`);

// Search users by criteria (name, phone, address, ID)
const search = async (query) =>
  instance.post('/api/private/customers/search', { query });

// Filter users by role
const filterByRole = async (role) =>
  instance.post('/api/private/customers/filter', { role });

export {
  getAll,
  getAllBasic,
  add,
  getById,
  update,
  remove,
  search,
  filterByRole
};
