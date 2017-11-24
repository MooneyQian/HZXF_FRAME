using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Common
{
    public class Define
    {
        public static string[] arrayManager = { "管理员" };


        public static string[] arrayCustomerManager = { "客户管理员" };
        public static string[] arrayCustomerOperator = { "客户操作员" };

        /// <summary>
        /// 配送单列表地址
        /// </summary>
        public static string DELIVER_MOBILE_LIST = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxa9a73e3d7c7f86ec&redirect_uri=http%3A%2F%2Fservice.hzxfkj.com%3A8888%2FMain%2FDeliver%2FMobileList&response_type=code&scope=snsapi_base&state=STATE#wechat_redirect";

        /// <summary>
        /// 维修单列表地址
        /// </summary>
        public static string REPAIRRECORD_MOBILE_LIST = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxa9a73e3d7c7f86ec&redirect_uri=http%3a%2f%2fservice.hzxfkj.com%3a8888%2fMain%2fRepairRecord%2fMobileList&response_type=code&scope=snsapi_base&state=STATE#wechat_redirect";

        /// <summary>
        /// 二维码URL
        /// </summary>
        public static string QRCODEURL = "http://122.225.203.81:8091/twocode.aspx?type=1&id={0}";

        //调用接口 xml
        public static string reportXML =
          "<?xml version='1.0' encoding='utf-8'?>" +
          "<request>" +
            "<product name='1'/>" +
            "<function name='videoReport'/>" +
            "<params>" +
              "<eventTypeID>类型id</eventTypeID>" +
              "<mainTypeID>" +
              "  大类id" +
              "</mainTypeID>" +
              "<subTypeID>" +
              "  小类ID" +
              "</subTypeID>" +
              "<eventTypeName>" +
              "  类型名称" +
              "</eventTypeName>" +
              "<mainTypeName>" +
              "  大类名称" +
              "</mainTypeName>" +
              "<subTypeName>" +
              "  小类名称" +
              "</subTypeName>" +
              "<eventDesc>" +
              "  问题描述" +
              "</eventDesc>" +
              "<address>" +
              "  地址" +
              "</address>" +
              "<coordX>x坐标</coordX>" +
              "<coordY>" +
              "  y坐标" +
              "</coordY>" +
              "<reportType>" +
              "  上报类型标识" +
              "</reportType>" +
              "<licencePlate>" +
              "  车牌" +
              "</licencePlate>" +
              "<reportTime>" +
              "  上报时间" +
              "</reportTime>" +
              "<reportPicTotalNum>" +
              "  上报图片数量" +
              "</reportPicTotalNum>" +
              "<reportVideoTotalNum>" +
              "  上报视频数量" +
              "</reportVideoTotalNum>" +
              "<picUrlString>" +
              "  图片路径字符串" +
              "</picUrlString>" +
              "<videoUrlString>" +
              "  视频文件路径字符串" +
              "</videoUrlString>" +

            "</params>" +
          "</request>";

    }
}
