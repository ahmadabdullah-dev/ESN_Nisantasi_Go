import { useQuery } from "@tanstack/react-query";
import agent from "../api/agent";
import type { CurrentUserDto, UserDto } from "../types/user";

export const useCurrentUser = () =>
  useQuery<CurrentUserDto>({
    queryKey: ["currentUser"],
    queryFn: () => agent.get<CurrentUserDto>("/User/current").then((res) => res.data),
    staleTime: 5 * 60 * 1000, // 5 min
    retry: false,
  });

export const useGetUserByUsername = (userName: string) =>
  useQuery<UserDto>({
    queryKey: ["user", userName],
    queryFn: () => agent.get<UserDto>("/User", { params: { userName } }).then((res) => res.data),
    enabled: !!userName,
    staleTime: 5 * 60 * 1000, 
    retry: false,
  });
