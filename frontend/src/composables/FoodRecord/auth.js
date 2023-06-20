import AuthService from '@/services/AuthService'
import { reactive } from 'vue';
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'



export default function useAuth() {

    const errors = reactive({}) 

    const login = async (request) => {

        errors.value = {}
        await AuthService.login(request)
            .then(res => {
                console.log("res: ", res)
            })
            .catch(err => {
                const newErrors = err.response.data.errors;
                errors.value = newErrors
            })
    }

    const register = async (request) => {

        errors.value = {}
        await AuthService.register(request)
            .then(res => {
                console.log("res: ", res)
            })
            .catch(err => {
                const newErrors = err.response.data.errors;
                errors.value = newErrors
            })
    }
    return {
        register,login,errors
    }
}