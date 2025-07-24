import axios from "axios";
import type { AxiosError } from "axios";
import type { User } from "./authSlice";
import { API_URL } from "../../utils";

interface LoginResponse {
    result: {
        user: User;
        token: string;
    };
    isSuccess: boolean;
    statusCode: number;
    message: string;
}

interface RegisterResponse {
    result: User;
    isSuccess: boolean;
    statusCode: number;
    message: string;
}

export const loginAPI = async (credentials: {
    userName: string;
    password: string;
}): Promise<{ token: string; user: User }> => {
    const URL = API_URL + "/user/Login";

    try {
        const { data } = await axios.post<LoginResponse>(URL, credentials);

        if (data.isSuccess && data.result) {
            return {
                token: data.result.token,
                user: data.result.user,
            };
        }

        throw new Error(data.message || "Bir hata oluştu");
    } catch (err) {
        const error = err as AxiosError<{ message: string }>;

        const message = error.response?.data?.message || "Bir hata oluştu";

        throw new Error(message);
    }
};

export const registerAPI = async (formData: {
    name: string;
    surname: string;
    email: string;
    userName: string;
    password: string;
}): Promise<{ user: User }> => {
    const URL = API_URL + "/user/Register";

    try {
        const { data } = await axios.post<RegisterResponse>(URL, formData);

        if (data.isSuccess && data.result) {
            return {
                user: data.result,
            };
        }

        throw new Error(data.message || "Bir hata oluştu");
    } catch (err) {
        const error = err as AxiosError<{ message: string }>;

        const message = error.response?.data?.message || "Bir hata oluştu";

        throw new Error(message);
    }
};
