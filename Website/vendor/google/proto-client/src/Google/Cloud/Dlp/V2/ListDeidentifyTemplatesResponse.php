<?php
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: google/privacy/dlp/v2/dlp.proto

namespace Google\Cloud\Dlp\V2;

use Google\Protobuf\Internal\GPBType;
use Google\Protobuf\Internal\RepeatedField;
use Google\Protobuf\Internal\GPBUtil;

/**
 * Response message for ListDeidentifyTemplates.
 *
 * Generated from protobuf message <code>google.privacy.dlp.v2.ListDeidentifyTemplatesResponse</code>
 */
class ListDeidentifyTemplatesResponse extends \Google\Protobuf\Internal\Message
{
    /**
     * List of deidentify templates, up to page_size in
     * ListDeidentifyTemplatesRequest.
     *
     * Generated from protobuf field <code>repeated .google.privacy.dlp.v2.DeidentifyTemplate deidentify_templates = 1;</code>
     */
    private $deidentify_templates;
    /**
     * If the next page is available then the next page token to be used
     * in following ListDeidentifyTemplates request.
     *
     * Generated from protobuf field <code>string next_page_token = 2;</code>
     */
    private $next_page_token = '';

    public function __construct() {
        \GPBMetadata\Google\Privacy\Dlp\V2\Dlp::initOnce();
        parent::__construct();
    }

    /**
     * List of deidentify templates, up to page_size in
     * ListDeidentifyTemplatesRequest.
     *
     * Generated from protobuf field <code>repeated .google.privacy.dlp.v2.DeidentifyTemplate deidentify_templates = 1;</code>
     * @return \Google\Protobuf\Internal\RepeatedField
     */
    public function getDeidentifyTemplates()
    {
        return $this->deidentify_templates;
    }

    /**
     * List of deidentify templates, up to page_size in
     * ListDeidentifyTemplatesRequest.
     *
     * Generated from protobuf field <code>repeated .google.privacy.dlp.v2.DeidentifyTemplate deidentify_templates = 1;</code>
     * @param \Google\Cloud\Dlp\V2\DeidentifyTemplate[]|\Google\Protobuf\Internal\RepeatedField $var
     * @return $this
     */
    public function setDeidentifyTemplates($var)
    {
        $arr = GPBUtil::checkRepeatedField($var, \Google\Protobuf\Internal\GPBType::MESSAGE, \Google\Cloud\Dlp\V2\DeidentifyTemplate::class);
        $this->deidentify_templates = $arr;

        return $this;
    }

    /**
     * If the next page is available then the next page token to be used
     * in following ListDeidentifyTemplates request.
     *
     * Generated from protobuf field <code>string next_page_token = 2;</code>
     * @return string
     */
    public function getNextPageToken()
    {
        return $this->next_page_token;
    }

    /**
     * If the next page is available then the next page token to be used
     * in following ListDeidentifyTemplates request.
     *
     * Generated from protobuf field <code>string next_page_token = 2;</code>
     * @param string $var
     * @return $this
     */
    public function setNextPageToken($var)
    {
        GPBUtil::checkString($var, True);
        $this->next_page_token = $var;

        return $this;
    }

}

