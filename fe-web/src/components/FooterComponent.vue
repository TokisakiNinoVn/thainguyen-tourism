<template>
    <footer class="bg-white w-full max-w-[966px] mx-auto py-6 px-4 border-t border-gray-200">
    <div class="flex flex-col md:flex-row justify-between items-center gap-6">
        <!-- Logo Section -->
        <div class="flex items-center gap-3">
        <div class="bg-green-500 rounded-full w-10 h-10 flex items-center justify-center">
            <span class="text-white font-bold text-[17px]">TN</span>
        </div>
        <h2 class="text-green-700 font-bold text-[17px]">Thái Nguyên Travel</h2>
        </div>

        <!-- Navigation Links -->
        <nav class="flex flex-wrap gap-6">
        <router-link to="/places" class="text-gray-700 font-medium text-[13.6px] hover:text-green-600">Địa điểm</router-link>
        <router-link to="/blog" class="text-gray-700 font-medium text-[13.6px] hover:text-green-600">Blog</router-link>
        <router-link to="/about" class="text-gray-700 font-medium text-[13.6px] hover:text-green-600">About</router-link>
        <router-link to="/contact" class="text-gray-700 font-medium text-[13.6px] hover:text-green-600">Contact</router-link>
        </nav>

        <!-- Contact Info -->
        <div class="flex flex-col items-center md:items-end text-gray-700 text-[13.6px] font-medium">
        <p>Email: contact@thainguyentravel.com</p>
        <p>Phone: +84 123 456 789</p>
        </div>
    </div>
    <!-- Copyright -->
    <div class="mt-6 text-center text-gray-600 text-[12px]">
        <p>&copy; 2025 Thái Nguyên Travel. All rights reserved.</p>
    </div>
    </footer>
</template>

<script>
export default {
    name: "FooterComponent",
};
</script>

<style scoped>
/* Minimal custom CSS for specific overrides */
footer {
    font-family: 'Inter', sans-serif;
}

/* Ensure router-link doesn't add unwanted styles */
router-link {
    text-decoration: none;
}
</style>