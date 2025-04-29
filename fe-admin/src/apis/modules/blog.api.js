import instance from '@/apis/axiosConfig';

const getListBlogApi = async () => instance.get(`/api/admin/blog/all/`);

// delete blog
const deleteBlogApi = async (id) => instance.delete(`/api/admin/blog/delete/${id}`);

// get blog by id
const getBlogByIdApi = async (id) => instance.get(`/api/admin/blog/${id}`);

// create blog
const createBlogApi = async (data) => instance.post(`/api/admin/blog/create`, data);


// update blog
const updateBlogApi = async (id, data) => instance.put(`/api/admin/blog/update/${id}`, data);

export {
    getListBlogApi,
    deleteBlogApi,
    getBlogByIdApi,
    createBlogApi,
    updateBlogApi
};