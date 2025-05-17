import instance from '@/apis/axiosConfig';

const getVisitCountApi = async () => instance.get(`/api/admin/dashboard/visit-count`);
const getBlogCountApi = async () => instance.get(`/api/admin/dashboard/blog-count`);
const getUserCountApi = async () => instance.get(`/api/admin/dashboard/user-count`);
const getReviewCountApi = async () => instance.get(`/api/admin/dashboard/review-count`);
const getListFavoritePlacesApi = async () => instance.get(`/api/admin/dashboard/favorite-places`);


export {
    getVisitCountApi,
    getBlogCountApi,
    getUserCountApi,
    getReviewCountApi,
    getListFavoritePlacesApi
};