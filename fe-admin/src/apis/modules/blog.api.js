import instance from '@/apis/axiosConfig';

const getListBlogApi = async () => instance.get(`/api/admin/blog/list`);
const getDetailBlogByIdApi = async (id) => instance.get(`api/web/blog/details/${id}`);
const getBlogByIdApi = async (id) => instance.get(`/api/admin/blog/${id}`);

// delete blog
const deleteBlogApi = async (id) => instance.delete(`/api/admin/blog/delete/${id}`);

// get blog by id

// create blog
const createBlogApi = async (data) => instance.post(`/api/admin/blog/create`, data);


// update blog
const putApproveBlogApi = async (id) => instance.put(`/api/admin/blog/approve/${id}`);

export {
    getListBlogApi,
    getDetailBlogByIdApi,
    deleteBlogApi,
    getBlogByIdApi,
    createBlogApi,
    putApproveBlogApi,
};